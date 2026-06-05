using Npgsql;
using SAE2._01_Application_WPF.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Logique d'interaction pour UC_Participants1stPage.xaml
    /// </summary>
    public partial class UC_Participants1stPage : UserControl
    {
        private UserControl pagePrecedente;
        private int seanceID;
        private bool estResponsable;
        

        public UserControl PagePrecedente
        {
            get
            {
                return this.pagePrecedente;
            }

            set
            {
                this.pagePrecedente = value;
            }
        }

        public int SeanceID
        {
            get
            {
                return this.seanceID;
            }

            set
            {
                this.seanceID = value;
            }
        }

        public bool EstResponsable
        {
            get
            {
                return this.estResponsable;
            }

            set
            {
                this.estResponsable = value;
            }
        }

        public UC_Participants1stPage(bool estResponsable)
        {
            InitializeComponent();
            btnValiderInscription.Visibility = Visibility.Collapsed; 
            this.EstResponsable = estResponsable;
        }

        public UC_Participants1stPage(int seanceID, UserControl pagePrecedente)
        {
            InitializeComponent();
            this.SeanceID = seanceID;
            this.PagePrecedente = pagePrecedente;

            if (this.PagePrecedente is PlanningDuJour)
            {
                dgListeParticipants.ItemsSource = new Client().FindBySeance(seanceID);
                btnValiderInscription.Content = "Nouvelle Inscription";
            }
            else if (this.PagePrecedente is UC_Séance)
            {
                btnVoirSeances.Visibility= Visibility.Collapsed;
            }
            else
            {
                dgListeParticipants.ItemsSource = new Client().FindAll();
                btnValiderInscription.Content = "Valider l'inscription";
            }

        }

        private void TextBox_Focus(object sender, RoutedEventArgs e)
        {
            TextBox box = (TextBox)sender;
            if (box.Text == "Recherche..." || box.Text == "Naissance..." || box.Text == "Adresse...")
            {
                box.Text = "";
                box.Foreground = Brushes.Black;
            }
        }

        private void TextBox_NoFocus(object sender, RoutedEventArgs e)
        {
            TextBox box = (TextBox)sender;
            if (string.IsNullOrWhiteSpace(box.Text))
            {
                box.Foreground = Brushes.Gray;
                if (box == txtFiltreRecherche) box.Text = "Recherche...";
                if (box == txtFiltreNaissance) box.Text = "Naissance...";
                if (box == txtFiltreAdresse) box.Text = "Adresse...";
            }
        }

       

        private void btnOuvrirCreation_Click(object sender, RoutedEventArgs e)
        {
            MainWindow fenetrePrincipale = (MainWindow)Window.GetWindow(this);

            if (fenetrePrincipale != null)
            {
                UC_Participants2ndPage nouvellePage = new UC_Participants2ndPage(this);

                fenetrePrincipale.MainContainer.Content = nouvellePage;
            }
        }
        private void Filtrer(object sender, TextChangedEventArgs e)
        {
            if (dgListeParticipants == null) return;

            ICollectionView vue = CollectionViewSource.GetDefaultView(dgListeParticipants.ItemsSource);
            if (vue == null) return;

            string recherche = TexteFiltre(txtFiltreRecherche, "Recherche...");
            string naissance = TexteFiltre(txtFiltreNaissance, "Naissance...");
            string adresse = TexteFiltre(txtFiltreAdresse, "Adresse...");

            vue.Filter = obj =>
            {
                if (!(obj is Client c)) return false;

                bool okRecherche = recherche == ""
                    || (c.Nom ?? "").ToLower().Contains(recherche)
                    || (c.Prenom ?? "").ToLower().Contains(recherche)
                    || (c.Mail ?? "").ToLower().Contains(recherche)
                    || (c.Telephone ?? "").ToLower().Contains(recherche);


                bool okNaissance = naissance == ""
                    || c.DateNaissance.ToString("dd/MM/yyyy").Contains(naissance);

                bool okAdresse = adresse == ""
                    || (c.Adresse ?? "").ToLower().Contains(adresse)
                    || (c.Ville ?? "").ToLower().Contains(adresse)
                    || (c.CodePostal ?? "").ToLower().Contains(adresse);

                return okRecherche && okNaissance && okAdresse;
            };
        }


        private string TexteFiltre(TextBox box, string placeholder)
        {
            string t = box.Text.Trim();
            return (t == placeholder) ? "" : t.ToLower();
        }

        private void btnValiderInscription_Click(object sender, RoutedEventArgs e)
        {
            MainWindow fenetre = (MainWindow)Window.GetWindow(this);
            if (fenetre == null) return;

            if (this.pagePrecedente is PlanningDuJour)
            {
                UC_Participants1stPage pageSelectionClient = new UC_Participants1stPage(this.SeanceID, this);
                fenetre.MainContainer.Content = pageSelectionClient;
            }
            else
            {
                if (dgListeParticipants.SelectedItem is Client client)
                {
                    var connexion = DataAccess.GetConnection();
                    string cmd = "INSERT INTO INSCRIPTION (SEANCE_ID, CLIENT_ID, DATE_INSCRIPTION) " +
                                 "VALUES (@seance, @client, @date);";
                    int id_client = client.IdClient;
                    int id_seance = this.SeanceID;
                    DateOnly dateInscription = DateOnly.FromDateTime(DateTime.Today);


                    string checkCmd = "SELECT COUNT(*) FROM INSCRIPTION WHERE SEANCE_ID = @seance AND CLIENT_ID = @client";
                    using (NpgsqlCommand check = new NpgsqlCommand(checkCmd, connexion))
                    {
                        check.Parameters.AddWithValue("@seance", id_seance);
                        check.Parameters.AddWithValue("@client", id_client);

                        int count = Convert.ToInt32(check.ExecuteScalar());
                        if (count > 0)
                        {
                            MessageBox.Show("Ce client est déjà inscrit à cette séance.", "Inscription existante", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                    }

                    try
                    {
                        using (NpgsqlCommand commande = new NpgsqlCommand(cmd, connexion))
                        {
                            commande.Parameters.AddWithValue("@seance", id_seance);
                            commande.Parameters.AddWithValue("@client", id_client);
                            commande.Parameters.AddWithValue("@date", dateInscription);

                            int lignes = commande.ExecuteNonQuery();
                            if (lignes > 0)
                            {
                                string bilan = "L'inscription a été enregistrée avec succès !\n\n" +
                                               "--- BILAN DE L'INSCRIPTION ---\n" +
                                               $"Participant : {client.Prenom} {client.Nom.ToUpper()}\n" +
                                               $"ID Client   : {client.IdClient}\n" +
                                               $"ID Séance   : {id_seance}\n" +
                                               $"Date inscription   : {dateInscription.ToString("dd/MM/yyyy")}\n" +
                                               "-----------------------------\n";

                                MessageBox.Show(bilan, "Succès - Bilan Inscription", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erreur BDD directe : " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    if (this.pagePrecedente is UC_Participants1stPage pageParticipantsPrécédente)
                    {
                        pageParticipantsPrécédente.dgListeParticipants.ItemsSource = new Client().FindBySeance(this.SeanceID);
                        fenetre.MainContainer.Content = pageParticipantsPrécédente;
                    }
                    else
                    {
                        fenetre.MainContainer.Content = this.pagePrecedente;
                    }
                }
                else
                {
                    MessageBox.Show("Sélectionne d'abord un client dans la liste pour l'inscrire.",
                                    "Aucun client sélectionné",
                                    MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void btnVoirSeances_Click(object sender, RoutedEventArgs e)
        {
            if (dgListeParticipants.SelectedItem is Client client)
            {
                MainWindow fenetre = (MainWindow)Window.GetWindow(this);
                if (fenetre != null)
                    fenetre.MainContainer.Content = new UC_Séance(this.EstResponsable, client.IdClient, this);
            }
            else
            {
                MessageBox.Show("Sélectionne d'abord un client.",
                                "Aucune client sélectionnée",
                                MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
