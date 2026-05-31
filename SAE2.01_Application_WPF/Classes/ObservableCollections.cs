using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SAE2._01_Application_WPF.Classes
{
    public class ObservableCollections
    {
        private ObservableCollection<Categorie> lesCategories;
        private ObservableCollection<Abonnement> lesAbonnements;
        private ObservableCollection<Client> lesClients;
        private ObservableCollection<Cours> lesCours;
        private ObservableCollection<Entraineur> lesEntraineurs;
        private ObservableCollection<Salle> lesSalles;
        private ObservableCollection<Seance> lesSeances;

        public ObservableCollections()
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
                return;

            this.LesCategories = new ObservableCollection<Categorie>(new Categorie().FindAll()); ;
            this.LesAbonnements = new ObservableCollection<Abonnement>(new Abonnement().FindAll()); ;
            this.LesClients = new ObservableCollection<Client>(new Client().FindAll()); ;
            this.LesCours = new ObservableCollection<Cours>(new Cours().FindAll()); ;
            this.LesEntraineurs = new ObservableCollection<Entraineur>(new Entraineur().FindAll()); ;
            this.LesSalles = new ObservableCollection<Salle>(new Salle().FindAll()); ;
            this.LesSeances = new ObservableCollection<Seance>(new Seance().FindAll()); ;
        }

        public ObservableCollection<Categorie> LesCategories
        {
            get
            {
                return this.lesCategories;
            }

            set
            {
                this.lesCategories = value;
            }
        }

        public ObservableCollection<Abonnement> LesAbonnements
        {
            get
            {
                return this.lesAbonnements;
            }

            set
            {
                this.lesAbonnements = value;
            }
        }

        public ObservableCollection<Client> LesClients
        {
            get
            {
                return this.lesClients;
            }

            set
            {
                this.lesClients = value;
            }
        }

        public ObservableCollection<Cours> LesCours
        {
            get
            {
                return this.lesCours;
            }

            set
            {
                this.lesCours = value;
            }
        }

        public ObservableCollection<Entraineur> LesEntraineurs
        {
            get
            {
                return this.lesEntraineurs;
            }

            set
            {
                this.lesEntraineurs = value;
            }
        }

        public ObservableCollection<Salle> LesSalles
        {
            get
            {
                return this.lesSalles;
            }

            set
            {
                this.lesSalles = value;
            }
        }

        public ObservableCollection<Seance> LesSeances
        {
            get
            {
                return this.lesSeances;
            }

            set
            {
                this.lesSeances = value;
            }
        }
    }
}
