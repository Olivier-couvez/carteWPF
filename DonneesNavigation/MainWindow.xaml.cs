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
        string[] lesDonneesLignes = new string[];
        

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
