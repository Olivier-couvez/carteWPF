using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Net.Sockets;
using System.Windows.Threading;
using System.Text.RegularExpressions;

namespace DonneesNavigation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private RecupNav recupdonnees;
        DispatcherTimer timerRecup = new DispatcherTimer();
        string lesDonnees = "";
        string[] lesDonneesLignes = new string[1];
        string[] laLigne = new string[1];
        string heure;
        string lat, lon, latSens, lonSens;


        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnDemarre_Click(object sender, RoutedEventArgs e)
        {
            connexion();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            LectureCommande();
        }

        private void LectureCommande()
        {
            try
            {
                lesDonnees = recupdonnees.Lecture();

                lesDonneesLignes = Regex.Split(lesDonnees, "\n\\s*");
                for (int i = 0; i<lesDonneesLignes.Length;i++)
                {
                    laLigne = Regex.Split(lesDonneesLignes[i], ",");
                    if (laLigne[0] == "$GPGGA")
                    {
                        heure = laLigne[1];
                        lat = laLigne[2];
                        latSens = laLigne[3];
                        lon = laLigne[4];
                        lonSens = laLigne[5];
                    }
                }

                
            }
            catch
            {
                timerRecup.Stop();
                MessageBoxResult resultat;
                lblconnexion.Content = "Connexion perdu !";
                resultat = MessageBox.Show("Il semblerai que nous ayons perdu la connexion, réessayer ?", "Erreur connexion", MessageBoxButton.OKCancel, MessageBoxImage.Error);

                if (resultat == MessageBoxResult.OK)
                {
                    connexion();
                }
                else
                {
                    btnQuitter.Focus();
                }
            }


            txtbHeure.Text = heure;
            txtbLat.Text = lat + " - " + latSens;
            txtbLon.Text = lon + " - " + lonSens;
            txtblAffichageRecup.Text = txtblAffichageRecup.Text + heure + " / " + lat + " - " + latSens + " / " + lon + " - " + lonSens + "\n";

        }


        private void connexion()
        {
            recupdonnees = new RecupNav("localhost", 3000);
            try
            {
                timerRecup.IsEnabled = true;


                timerRecup.Tick += timer_Tick;

                timerRecup.Interval = TimeSpan.FromSeconds(0);
                timerRecup.Start();
                lblconnexion.Content = "Connexion Ok !";

            }
            catch
            {
                btnQuitter.Focus();
            }
        }

        private void btnQuitter_Click(object sender, RoutedEventArgs e)
        {
            timerRecup.Stop();
            this.Close();
        }
    }
}
