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
    /// Логика взаимодействия для CreateNewChannelDialog.xaml
    /// </summary>
    public partial class CreateNewChannelDialog : Window
    {
        public CreateNewChannelDialog()
        {
            InitializeComponent();
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            string name = ChNameTB.Text;
            string desc = new TextRange(ChDescTB.Document.ContentStart, ChDescTB.Document.ContentEnd).Text;

            App.GetCurrent().CreateChannelAndSubcribeTo(name, desc);
            this.Close();
        }
    }
}
