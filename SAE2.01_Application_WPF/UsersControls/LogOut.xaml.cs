using SAE2._01_Application_WPF.Classes;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SAE2._01_Application_WPF.UsersControls
{
    /// <summary>
    /// Logique d'interaction pour LogOut.xaml
    /// </summary>
    public partial class LogOut : UserControl
    {
        private MainWindow mainWindow;
        public LogOut(MainWindow mainWindow)
        {
            InitializeComponent();
            this.MainWindow = mainWindow;
        }

        public MainWindow MainWindow
        {
            get
            {
                return this.mainWindow;
            }

            set
            {
                this.mainWindow = value;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;

            switch (clickedButton.Name)
            {
                case "btnNON":
                    MainWindow.MainGrid.Children.Remove(this);
                    break;
                case "btnOUI":
                    DataAccess.CloseConnection();
                    MainWindow.MainGrid.Children.Remove(this);
                    MainWindow.MainGrid.Children.Add(new Login(this.MainWindow));
                    break;
            }
        }
    }
}
