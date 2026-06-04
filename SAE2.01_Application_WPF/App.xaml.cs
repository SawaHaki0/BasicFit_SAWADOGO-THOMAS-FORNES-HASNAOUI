using SAE2._01_Application_WPF.Classes;
using System.Configuration;
using System.Data;
using System.Windows;

namespace SAE2._01_Application_WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            this.Exit += App_Exit;
        }
        private void App_Exit(object sender, ExitEventArgs e)
        {
            DataAccess.CloseConnection();
        }
    }

}
