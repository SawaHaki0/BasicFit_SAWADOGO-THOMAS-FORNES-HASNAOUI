using System;
using System.Collections.Generic;
using System.Globalization;
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
	/// Logique d'interaction pour PlanningDuJour.xaml
	/// </summary>
	public partial class PlanningDuJour : UserControl
	{
		public PlanningDuJour()
		{
			InitializeComponent();

			var culture = new CultureInfo("fr-FR");
			string date = DateTime.Now.ToString("dddd, MMMM dd, yyyy", culture);
			DateLabel.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(date);
		}
	}
}
