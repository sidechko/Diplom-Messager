using MessagerClient.utils;
using MessagerClient.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WebSocketSharp;

namespace MessagerClient.WS
{
    public class WSClient
    {
        private WebSocket? Client;
        private int? sessionId;
        private string? url;
        private static readonly StompMessageSerializer serializer = new();
        private IWSMessageHandler? messageHandler;
        public bool ConnectedToServer = false;

        public WSClient Initilize()
        {
            if (url is null)
                throw new MessagerHardClientException("url is null");

            Client?.Close();

            Client = new WebSocket(url);
            
            return this;
        }

        public void Connect()
        {
            if (Client is null)
                throw new MessagerHardClientException("WS client not initilized");

            if (sessionId is null)
                throw new MessagerHardClientException("sessionId is null");

            if (messageHandler is null)
                throw new MessagerHardClientException("Message handler is null");

            Client.OnOpen += (s, e) =>
            {
                var connect = new StompMessage("CONNECT");
                connect["accept-version"] = "1.1";
                connect["heart-beat"] = "10000,10000";
                Client.Send(serializer.Serialize(connect));
            };

            Client.OnError += (s, e) => { Console.WriteLine("WS ERROR: " + e.Message); };
            Client.OnMessage += (s, e) => messageHandler.onMessageResive(serializer.Deserialize(e.Data));

            Client.Connect();
        }

        public WSClient SetURL(string url)
        {
            this.url = url;
            return this;
        }

        public WSClient SetSessionId(int sessionId)
        {
            this.sessionId = sessionId;
            return this;
        }

        public int GetSessionId()
        {
            return this.sessionId.HasValue ? this.sessionId.Value : -1;
        }

        public WSClient SetMessageHandler(IWSMessageHandler handler)
        {
            this.messageHandler = handler;
            return this;
        }

        public void SubscribeTo(string distination)
        {
            if (Client is null)
                throw new MessagerHardClientException("Client is null. Sub canceled");

            var subscribeMessage = new StompMessage(StompFrame.SUBSCRIBE);
            subscribeMessage["id"] = "sub-" + sessionId;
            subscribeMessage["destination"] = "/state" + distination;
            Client.Send(serializer.Serialize(subscribeMessage));
        }

        public void UnsubscribeFrom(string distination)
        {
            if (Client is null)
                throw new MessagerHardClientException("Client is null. Unsub canceled");

            var unsubscribeMessage = new StompMessage(StompFrame.UNSUBSCRIBE);
            unsubscribeMessage["id"] = "sub-" + sessionId;
            unsubscribeMessage["destination"] = "/state"+distination;

            Client.Send(serializer.Serialize(unsubscribeMessage));
        }

        public void Send<T>(string destination, T body)
        {
            if (Client is null)
                throw new MessagerHardClientException("Client is null. Send canceled");

            var message = new StompMessage(StompFrame.SEND, JsonHelper.Serialize(body));
            #pragma warning disable CS8601 
            message["sessionId"] = sessionId.ToString();
            #pragma warning restore CS8601
            message["id"] = "sub-" + sessionId;
            message["destination"] = "/app" + destination;

            Client.Send(serializer.Serialize(message));
        }

        public void Disconnect()
        {
            Client?.Close();
        }
    }
}
