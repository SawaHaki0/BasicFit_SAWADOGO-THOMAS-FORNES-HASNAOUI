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
    /// Logique d'interaction pour UC_SeanceModifier.xaml
    /// </summary>
    public partial class UC_SeanceModifier : UserControl
    {
        private Seance seance;
        private UserControl pagePrec;
        public UC_SeanceModifier(Seance seance, UserControl pagePrec)
        {
            InitializeComponent();
            this.Seance = seance;
            this.PagePrec = pagePrec;
        }

        public Seance Seance
        {
            get
            {
                return this.seance;
            }

            set
            {
                this.seance = value;
            }
        }

        public UserControl PagePrec
        {
            get
            {
                return this.pagePrec;
            }

            set
            {
                this.pagePrec = value;
            }
        }
    }
}
