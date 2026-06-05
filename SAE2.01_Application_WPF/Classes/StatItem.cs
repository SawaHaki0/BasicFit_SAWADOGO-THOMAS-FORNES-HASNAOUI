using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE2._01_Application_WPF.Classes
{
    public class StatItem
    {
        public string Libelle { get; set; }
        public int Inscrits { get; set; }
        public int Places { get; set; }
        public double Taux { get; set; }       // 0–100 (pour la barre)
        public string Detail { get; set; }     // ex : "75 % (45/60)"
    }
}