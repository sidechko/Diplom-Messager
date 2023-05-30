using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace MessagerClient
{
    /// <summary>
    /// Логика взаимодействия для AppendUserToChannel.xaml
    /// </summary>
    public partial class AppendUserToChannel : Window
    {
        public AppendUserToChannel()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            App.GetCurrent().AddUserToCurrentChannelAsName(UserNameTB.Text);
            this.Close();
        }
    }
}
