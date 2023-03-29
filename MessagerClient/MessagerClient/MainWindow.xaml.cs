using MessagerClient.models;
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

        private void OnChangeChannel(int channelId, bool isMessage)
        {
            CurrentDispacher.Invoke(() =>
            {
                if (isMessage && CurrentApp.IsSelectedChannel(channelId))
                    CurrentChannelMessages.Add(CurrentApp.GetLastAdded());
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
            CurrentApp.SendMessage(text);
        }
    }
}
