using SAE2._01_Application_WPF.UsersControls;
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

namespace SAE2._01_Application_WPF
{
    /// <summary>
    /// Logique d'interaction pour Interface_ResponsableDuClub.xaml
    /// </summary>
    public partial class Interface_ResponsableDuClub : UserControl
    {
        private MainWindow mainWindow;

        public Interface_ResponsableDuClub(MainWindow mainWindow)
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

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;
            StackPanel parentPanel = (StackPanel)clickedButton.Parent;
            foreach (var child in parentPanel.Children)
            {
                if (child is Button btn)
                {
                    btn.FontWeight = FontWeights.Normal;
                    btn.Foreground = (Brush)new BrushConverter().ConvertFromString("#000000");
                    btn.Background = (Brush)new BrushConverter().ConvertFromString("#EBF3FF");
                    btn.BorderThickness = new Thickness(0);
                    btn.BorderBrush = Brushes.Transparent;
                }
            }
            clickedButton.FontWeight = FontWeights.Bold;
            clickedButton.Foreground = (Brush)new BrushConverter().ConvertFromString("#9E4300");
            clickedButton.Background = (Brush)new BrushConverter().ConvertFromString("#CCDFFF");
            clickedButton.BorderThickness = new Thickness(5, 0, 0, 0);
            clickedButton.BorderBrush = (Brush)new BrushConverter().ConvertFromString("#9E4300");

            switch (clickedButton.Name)
            {
                case "menuPlanningJour":
                    MainWindow.MainContainer.Content = new PlanningDuJour();
                    break;
                case "menuCours":
                    MainWindow.MainContainer.Content = new PlanningDuJour(); 
                    break;
                case "menuParticipants":
                    MainWindow.MainContainer.Content = new UC_Participants1stPage(); 
                    break;
                case "menuCategories":
                    MainWindow.MainContainer.Content = new UCCategorie();
                    break;
                case "menuGererEntraineurs":
                    MainWindow.MainContainer.Content = new UCEntraineurs(); 
                    break;

            }
        }
    }
}
