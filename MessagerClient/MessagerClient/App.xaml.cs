using MessagerClient.models;
using MessagerClient.REST;
using MessagerClient.utils;
using MessagerClient.Utils;
using MessagerClient.WS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Windows;

namespace MessagerClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    /// 

    public partial class App : Application, IWSMessageHandler
    {
        private User? AppUser = null;
        public User GetAppUser() { return AppUser; }
        private int? SelectedChannel = null;

        private RESTClient restClient = new RESTClient().SetURL("http://localhost:8080/api").Initilize();
        private WSClient wsClient = new WSClient().SetURL("ws://localhost:8080/ws/websocket").Initilize();

        private Dictionary<int, User> UserPool = new();
        private Dictionary<int, Channel> ChannelPool = new();
        private Dictionary<int, Message> MessagePool = new();
        private Dictionary<int, ICollection<int>> ChannelMessagesDict = new();
        private Message LastAdded = null;

        public delegate void AppNotifyHandler(int id, bool message=false);
        public event AppNotifyHandler? UserChangeNotify = null;
        public event AppNotifyHandler? ChannelChangeNotify = null;

        protected override void OnExit(ExitEventArgs e)
        {
            wsClient.Disconnect();
            base.OnExit(e);
        }

        public void Login(User user)
        {
            if (AppUser is not null)
                throw new MessagerSoftClientException("you are already logged in");

            string response = restClient.SendAndResiveRequest(HttpMethod.Post, "/users/login", JsonHelper.Serialize(user));
            LoginResponse result;
            try
            {
                result = JsonHelper.Deserialize<LoginResponse>(response);
            }
            catch (MessagerDeserializeException ex)
            {
                throw new MessagerHardClientException(JsonHelper.Deserialize<ServerExceptionMessage>(response).Message, ex);
            }

            if (!result.SessionId.HasValue)
                throw new MessagerHardClientException("session id not found");

            user.Update(result.User);
            AppUser = user;

            restClient.SetSessionId(result.SessionId.Value);
            wsClient.SetSessionId(result.SessionId.Value).SetMessageHandler(this);

            AppendToOrEditUserPool(AppUser);
        }

        public void Register(User user)
        {
            if (AppUser is not null)
                throw new MessagerSoftClientException("you are already logged in");

            string response = restClient.SendAndResiveRequest(HttpMethod.Post, "/users/create", JsonHelper.Serialize(user));
            LoginResponse result;
            try
            {
                result = JsonHelper.Deserialize<LoginResponse>(response);
            }
            catch (MessagerDeserializeException ex)
            {
                throw new MessagerHardClientException(JsonHelper.Deserialize<ServerExceptionMessage>(response).Message, ex);
            }

            if (!result.SessionId.HasValue)
                throw new MessagerHardClientException("session id not found");

            user.Update(result.User);
            AppUser = user;

            restClient.SetSessionId(result.SessionId.Value);
            wsClient.SetSessionId(result.SessionId.Value).SetMessageHandler(this);

            AppendToOrEditUserPool(AppUser);
        }

        public void PrepareWS()
        {
            if (AppUser is null)
                throw new MessagerHardClientException("Messager user is null");
            if (AppUser.Id is null)
                throw new MessagerHardClientException("Messager user id is null");

            wsClient.Connect();

            string response = restClient.SendAndResiveRequest(HttpMethod.Get, "/users/"+AppUser.Id+ "/getChannels");
            List<Channel> channels;
            try
            {
                channels = JsonHelper.DeserializeList<Channel>(response).ToList();
            }
            catch (MessagerDeserializeException ex)
            {
                throw new MessagerHardClientException(JsonHelper.Deserialize<ServerExceptionMessage>(response).Message, ex);
            }
            wsClient.SubscribeTo("/user/" + AppUser.Id + "/append_to_channel");
            wsClient.SubscribeTo("/user/" + AppUser.Id + "/remove_from_channel");
            channels.ForEach(c=> {
                try
                {
                    Channel channel = AppendToOrEditChannelPool(c);
                    wsClient.SubscribeTo("/channel/" + channel.Id + "/delete");
                    wsClient.SubscribeTo("/channel/" + channel.Id + "/msg/send");
                    wsClient.SubscribeTo("/channel/" + channel.Id + "/msg/change");
                    wsClient.SubscribeTo("/channel/" + channel.Id + "/msg/delete");
                }
                catch { Console.WriteLine("SOMTHING WRONG"); }
            });
        }

        public void SendMessage(string content)
        {
            if (ChannelPool.Count == 0)
                return;
            if (SelectedChannel is null)
                return;
            Channel c = ChannelPool[SelectedChannel.Value];
            wsClient.Send<Message>("/channel/"+c.Id+"/send_message", new Message()
            {
                Sender = AppUser,
                Channel = ChannelPool.First().Value,
                Content = content
            });
        }

        public void LoadCurrentChannelMessages(int start, int count)
        {
            if (SelectedChannel is null)
                return;
            LoadChannelMessages(SelectedChannel.Value, start, count);
        }

        public void LoadChannelMessages(int channel, int start=0, int count=0)
        {
            string response;
            if (start < 0)
                start = 0;
            if (count > 0)
                response = restClient.SendAndResiveRequest(HttpMethod.Get, "/channels/" + channel + "/getMessages/from" + start + "/to" + count);
            else
                response = restClient.SendAndResiveRequest(HttpMethod.Get, "/channels/" + channel + "/getMessagesAll");
            List <Message> messages;
            try
            {
                messages = JsonHelper.DeserializeList<Message>(response).ToList();
            }
            catch (MessagerDeserializeException ex)
            {
                throw new MessagerSoftClientException(JsonHelper.Deserialize<ServerExceptionMessage>(response).Message, ex);
            }
            messages.ForEach(m => AppendToOrEditMessagePool(m));
        }

        //
        // work with UserPool
        //
        private User AppendToOrEditUserPool(User user)
        {
            if (!user.Id.HasValue)
                throw new MessagerModelException("user entity does not have id");

            if (!UserPool.ContainsKey(user.Id.Value))
                UserPool[user.Id.Value] = user;
            else
                UserPool[user.Id.Value].Update(user);

            UserChangeNotify?.Invoke(user.Id.Value);

            return UserPool[user.Id.Value];
        }
        private bool RemoveFromUserPool(int id)
        {
            UserChangeNotify?.Invoke(id);
            return UserPool.Remove(id);
        }
        private bool RemoveFromUserPool(User user)
        {
            if (!user.Id.HasValue)
                throw new MessagerModelException("user entity does not have id");
            return RemoveFromUserPool(user.Id.Value);
        }

        //
        // Work with ChannelMessagesDict
        //
        private void AppendToChannelMessagesDict(Channel channel, bool needNotify = false)
        {
            if (channel.Id is null)
                throw new MessagerModelException("channel entity does not have id");
            if(needNotify)
                ChannelChangeNotify?.Invoke(channel.Id.Value, false);
            if (!ChannelMessagesDict.ContainsKey(channel.Id.Value))
                ChannelMessagesDict[channel.Id.Value] = new SortedSet<int>();
        }
        private void AppendToChannelMessagesDict(Message message)
        {
            if (message.Id is null)
                throw new MessagerModelException("message entity does not have id");
            if (message.Channel is null)
                throw new MessagerModelException("message entity does not have channel");
            if (message.Channel.Id is null)
                throw new MessagerModelException("message channel entity does not have id");
            AppendToChannelMessagesDict(message.Channel);
            ChannelMessagesDict[message.Channel.Id.Value].Add(message.Id.Value);
            LastAdded = message;
            ChannelChangeNotify?.Invoke(message.Channel.Id.Value, true);
        }
        private bool RemoveFromChannelMessageDict(int channelId, int messageId)
        {
            ChannelChangeNotify?.Invoke(channelId, true);
            return ChannelMessagesDict[channelId].Remove(messageId);
        }
        private bool RemoveFromChannelMessageDict(int channelId)
        {
            ChannelChangeNotify?.Invoke(channelId);
            return ChannelMessagesDict.Remove(channelId);
        }
        private bool RemoveFromChannelMessageDict(Message message)
        {
            if (message.Id is null)
                throw new MessagerModelException("message entity does not have id");
            if (message.Channel is null)
                throw new MessagerModelException("message entity does not have channel");
            if (message.Channel.Id is null)
                throw new MessagerModelException("message channel entity does not have id");
            return RemoveFromChannelMessageDict(message.Channel.Id.Value, message.Id.Value);
        }

        //
        // work with ChannelPool
        //
        private Channel AppendToOrEditChannelPool(Channel channel)
        {
            if (!channel.Id.HasValue)
                throw new MessagerModelException("channel entity does not have id");

            if (!ChannelPool.ContainsKey(channel.Id.Value))
                ChannelPool[channel.Id.Value] = channel;
            else
                ChannelPool[channel.Id.Value].Update(channel);

            AppendToChannelMessagesDict(ChannelPool[channel.Id.Value], true);

            return ChannelPool[channel.Id.Value];
        }
        private bool RemoveFromChannelPool(int id)
        {
            RemoveFromChannelMessageDict(id);
            return ChannelPool.Remove(id);
        }
        private bool RemoveFromChannelPool(Channel channel)
        {
            if (!channel.Id.HasValue)
                throw new MessagerModelException("channele entity does not have id");
            return RemoveFromChannelPool(channel.Id.Value);
        }

        private void SubscribeToChannel(Channel channel)
        {
            if (channel.Id is null)
                throw new MessagerModelException("Channel id is not found");

            wsClient.SubscribeTo("/channel/" + channel.Id + "/delete");
            wsClient.SubscribeTo("/channel/" + channel.Id + "/msg/send");
            wsClient.SubscribeTo("/channel/" + channel.Id + "/msg/change");
            wsClient.SubscribeTo("/channel/" + channel.Id + "/msg/delete");
        }

        private void UnsubscribeFromChannel(Channel channel)
        {
            if (channel.Id is null)
                throw new MessagerModelException("Channel id is not found");

            wsClient.UnsubscribeFrom("/channel/" + channel.Id + "/delete");
            wsClient.UnsubscribeFrom("/channel/" + channel.Id + "/msg/send");
            wsClient.UnsubscribeFrom("/channel/" + channel.Id + "/msg/change");
            wsClient.UnsubscribeFrom("/channel/" + channel.Id + "/msg/delete");
        }

        //
        // work with MessagePool
        //
        private Message AppendToOrEditMessagePool(Message message, bool soft = true)
        {
            if (!message.Id.HasValue)
                throw new MessagerModelException("message entity does not have id");

            if (message.Sender is null || message.Sender.IsNull())
                throw new MessagerModelException("message entity does not have sender");

            if (!MessagePool.ContainsKey(message.Id.Value))
                MessagePool[message.Id.Value] = message;
            else
                MessagePool[message.Id.Value].Update(message, soft);
            AppendToChannelMessagesDict(message);
            AppendToOrEditUserPool(message.Sender);
            return MessagePool[message.Id.Value];
        }
        private bool RemoveFromMessagePool(int id)
        {
            return MessagePool.Remove(id);
        }
        private bool RemoveFromMessagePool(Message message)
        {
            if (!message.Id.HasValue)
                throw new MessagerModelException("message entity does not have id");
            RemoveFromChannelMessageDict(message);
            return RemoveFromMessagePool(message.Id.Value);
        }

        public List<Message> GetChannelMessages()
        {
            if (SelectedChannel is null)
                throw new MessagerSoftClientException("selected channel is not present");
            return ChannelMessagesDict[SelectedChannel.Value].Select(id => { return MessagePool[id]; }).ToList();
        }

        public List<Channel> GetChannels()
        {
            return ChannelPool.Values.ToList();
        }

        public Message GetLastAdded()
        {
            return this.LastAdded;
        }

        // get current app
        public static App GetCurrent()
        {
            return (App)Current;
        }

        public void SetSelectedChannel(int id)
        {
            SelectedChannel = id;
        }

        public bool IsSelectedChannel(int id)
        {
            if (SelectedChannel is null)
                return false;
            return SelectedChannel.Value == id;
        }

        //MESSAGE HANDLER!!!
        public void onMessageResive(StompMessage message)
        {
            string messageType = message.Command;
            if (messageType != StompFrame.MESSAGE)
                return;

            string destionation = message["destination"];
            if (destionation == null)
                throw new MessagerConnectionException("Stomp message strange format: "+message.ToString());
            string[] splitedPath = destionation.Remove(0, 1).Split("/");
            if(splitedPath[1].Equals("user"))
            {
                var body = JsonHelper.Deserialize<UserChannelLink>(message.Body);

                if (body.Channel is null)
                    throw new MessagerDeserializeException("Channel not found at body");

                if (splitedPath[3].Equals("append_to_channel"))
                {
                    AppendToOrEditChannelPool(body.Channel);
                    SubscribeToChannel(body.Channel);
                }
                else if (splitedPath[3].Equals("remove_from_channel"))
                {
                    RemoveFromChannelPool(body.Channel);
                    UnsubscribeFromChannel(body.Channel);
                }
                else
                {
                    throw new MessagerConnectionException("strange stomp message body: " + message.Body);
                }
            }
            else if (splitedPath[1].Equals("channel"))
            {
                if (splitedPath[3].Equals("delete"))
                {
                    var body = JsonHelper.Deserialize<Channel>(message.Body);

                    RemoveFromChannelPool(body);
                    UnsubscribeFromChannel(body);
                }
                else if (splitedPath[3].Equals("msg"))
                {
                    var body = JsonHelper.Deserialize<Message>(message.Body);

                    if (splitedPath[4].Equals("send")) 
                    {
                        AppendToOrEditMessagePool(body);
                    }
                    else if (splitedPath[4].Equals("change")) 
                    {
                        AppendToOrEditMessagePool(body, true);
                    }
                    else if (splitedPath[4].Equals("delete")) 
                    {
                        RemoveFromMessagePool(body);
                    }
                    else
                    {
                        throw new MessagerConnectionException("strange stomp message body: " + message.Body);
                    }
                }
            }
            else
            {
                throw new MessagerConnectionException("strange stomp message destination: "+destionation);
            }
        }
    }
}
