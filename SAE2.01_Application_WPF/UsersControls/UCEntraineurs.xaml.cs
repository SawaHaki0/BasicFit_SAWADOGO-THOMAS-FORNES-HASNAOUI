using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using SAE2._01_Application_WPF.Classes; // Permet d'accéder à la classe Entraineur

namespace SAE2._01_Application_WPF.UsersControls
{
    /// <summary>
    /// Logique d'interaction pour UCEntraineurs.xaml
    /// </summary>
    public partial class UCEntraineurs : UserControl
    {
        // Cette propriété publique va alimenter le ItemsSource du DataGrid dans le XAML
        public List<Entraineur> LesEntraineurs { get; set; }

        public UCEntraineurs()
        {
            InitializeComponent();

            // 1. On charge la liste de tous les entraîneurs depuis la base de données
            this.LesEntraineurs = new Entraineur().FindAll();

            // 2. On lie ce code C# au XAML grâce au DataContext
            this.DataContext = this;
        }

        // Gère le clic sur le bouton "Ajouter un entraîneur"
        private void btnAjout_click(object sender, RoutedEventArgs e)
        {
            MainWindow fenetrePrincipale = (MainWindow)Window.GetWindow(this);

            if (fenetrePrincipale != null)
            {
                // On instancie l'UserControl d'ajout
                UC_AjoutEntraineur nouvellePage = new UC_AjoutEntraineur();

                // On l'affiche proprement au centre de l'application dans le MainContainer
                fenetrePrincipale.MainContainer.Content = nouvellePage;
            }
        }

        private void btnSupprimer_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}