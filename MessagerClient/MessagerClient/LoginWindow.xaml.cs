using MessagerClient.models;
using MessagerClient.utils;
using MessagerClient.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MessagerClient
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            User user = new User();
            user.Login = LoginTB.Text;
            user.Password = PasswordTB.Password;
            try
            {
                App.GetCurrent().Login(user);
                MessageBox.Show("you have successfully logged in to your account");
                App.GetCurrent().PrepareWS();
                ShowMainWindow();
            }
            catch (MessagerSoftClientException exception)
            {
                MessageBox.Show(exception.Message, exception.GetType().Name);
                ShowMainWindow();
            }
            catch (MessagerBaseException exception)
            {
                MessageBox.Show(exception.Message, exception.GetType().Name);
            }
            catch (Exception exception)
            {
                MessageBox.Show("ERROR: " + exception.Message);
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            User user = new User();
            user.Login = LoginTB.Text;
            user.Password = PasswordTB.Password;
            try
            {
                App.GetCurrent().Register(user);
                MessageBox.Show("you have successfully registered");
                App.GetCurrent().PrepareWS();
                ShowMainWindow();
            }
            catch (MessagerSoftClientException exception)
            {
                MessageBox.Show(exception.Message, exception.GetType().Name);
            }
            catch (MessagerBaseException exception)
            {
                MessageBox.Show(exception.Message, exception.GetType().Name);
            }
            catch (Exception exception)
            {
                MessageBox.Show("ERROR: " + exception.Message);
            }
        }

        private void ShowMainWindow()
        {
            new MainWindow(this).Show();
            this.Hide();
        }
    }
}
