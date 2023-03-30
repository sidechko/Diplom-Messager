using MessagerClient.models;
using MessagerClient.utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace MessagerClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private Window Parent;
        private App CurrentApp;
        private List<Message> CurrentChannelMessages = new();
        private Dispatcher CurrentDispacher = Dispatcher.CurrentDispatcher;
        private Message? ToEdit = null;
        public MainWindow(Window parent)
        {
            InitializeComponent();
            this.Parent = parent;
            CurrentApp = App.GetCurrent();
            CurrentApp.ChannelChangeNotify += OnChangeChannel;
            OnChangeChannel(-1, false);
            Channels.DisplayMemberPath = "Name";
            this.Resources.Remove("CurUser");
            this.Resources.Add("CurUser", CurrentApp.GetAppUser().Login);
            UpdateMessageSource();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            this.Parent?.Show();
        }

        private void OnChangeChannel(int channelId, bool isMessage, int type = 0)
        {
            CurrentDispacher.Invoke(() =>
            {
                if (isMessage && CurrentApp.IsSelectedChannel(channelId))
                {
                    switch (type)
                    {
                        //ADD
                        case 0:
                            CurrentChannelMessages.Add(CurrentApp.GetLastAdded());
                            break;
                        //EDIT
                        case 1:
                            int index = CurrentChannelMessages.IndexOf(CurrentChannelMessages.Single(msg => msg.Id.Value == CurrentApp.GetLastAdded().Id.Value));
                            CurrentChannelMessages.RemoveAt(index);
                            CurrentChannelMessages.Insert(index, CurrentApp.GetLastAdded());
                            break;
                        //DELETE
                        case 2:
                            CurrentChannelMessages.Remove(CurrentChannelMessages.Single(msg => msg.Id.Value == CurrentApp.GetLastRemoved().Id.Value));
                            break;
                    }
                }
                else
                    Channels.ItemsSource = CurrentApp.GetChannels();
            });
            UpdateMessageSource();
        }

        private void LoadMessagesForCurrentChannel()
        {
            try
            {
                CurrentApp.LoadCurrentChannelMessages(CurrentChannelMessages.Capacity, 0);
            }
            catch {}
        }

        private void Channels_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedChannel = Channels.SelectedItem as Channel;
            if (selectedChannel is null)
                return;
            if (selectedChannel.Id is null)
                return;
            App.GetCurrent().SetSelectedChannel(selectedChannel.Id.Value);
            CurrentChannelMessages = CurrentApp.GetChannelMessages();
            if (CurrentChannelMessages.Count == 0)
                LoadMessagesForCurrentChannel();
            UpdateMessageSource();
        }

        private void UpdateMessageSource()
        {
            CurrentDispacher.Invoke(() =>
            {
                ChannelMessages.ItemsSource = null;
                ChannelMessages.ItemsSource = CurrentChannelMessages;
            });
        }

        private void SendMessage_Click(object sender, RoutedEventArgs e)
        {
            string text = new TextRange(MessageTextBox.Document.ContentStart, MessageTextBox.Document.ContentEnd).Text;
            MessageTextBox.Document.Blocks.Clear();
            if(ToEdit is null)
                CurrentApp.SendMessage(text);
            else
            {
                if (text.Length != 0)
                    CurrentApp.EditMessage(ToEdit.Id.Value, text);
                else
                    CurrentApp.DeleteMessage(ToEdit);
            }
        }

        private void TryEditMessage(object sender, RoutedEventArgs e)
        {
            Message? msg = ChannelMessages.SelectedItem as Message;
            if (msg is null)
                return;
            try
            {
                CurrentApp.EditMessage(msg.Id.Value, "", true);
                ToEdit = msg;

                MessageTextBox.Document.Blocks.Clear();
                MessageTextBox.AppendText(msg.Content);
            }
            catch(MessagerSoftClientException ex) {
                MessageBox.Show(ex.Message, ex.GetType().Name);
            }
            catch(Exception ex)
            {
                MessageBox.Show("ERROR: "+ex.Message, ex.GetType().Name);
            }
        }

        private void TryDeleteMessage(object sender, RoutedEventArgs e)
        {
            Message? msg = ChannelMessages.SelectedItem as Message;
            if (msg is null)
                return;
            try { 
                CurrentApp.DeleteMessage(msg);
            }
            catch(MessagerBaseException ex){
                MessageBox.Show(ex.Message, ex.GetType().Name);
            }
            catch(Exception ex) {
                MessageBox.Show("ERROR: "+ex.Message, ex.GetType().Name);
            }
        }
    }
}
