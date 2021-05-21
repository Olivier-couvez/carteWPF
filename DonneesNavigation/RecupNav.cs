using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;
using System.Windows;

namespace DonneesNavigation
{
    class RecupNav
    {
        private int Port;
        private string AdrTCP;
        private Byte[] Commande;
        TcpClient SocketClient;
        NetworkStream NetStream;

        public RecupNav(string AdrTCP, int Port)
        {

            try
            {
                SocketClient = new TcpClient();
                SocketClient.Connect(AdrTCP, Port);
                NetStream = SocketClient.GetStream();
            }

            catch (ArgumentOutOfRangeException err)
            {
                MessageBox.Show("Une erreur est survenue : " + err.Message +
                "\nImpossible de se connecter à l'hote, vérifiez l'adresse IP ? ");
            }

            catch (SocketException err)
            {
                MessageBox.Show("Une erreur est survenue : " + err.Message + 
                "\nImpossible de se connecter à l'hote, vérifiez l'adresse IP ? ");
            }          
        }

        public int Port1 { get => Port; set => Port = value; }
        public string AdrTCP1 { get => AdrTCP; set => AdrTCP = value; }

        public string Lecture()
        {
            string returndata = "";

            if (NetStream.CanRead)
            {
                // Reads NetworkStream into a byte buffer.
                byte[] bytes = new byte[SocketClient.ReceiveBufferSize];

                // Read can return anything from 0 to numBytesToRead.
                // This method blocks until at least one byte is read.
                NetStream.Read(bytes, 0, (int)SocketClient.ReceiveBufferSize);

                // Returns the data received from the host to the console.
                returndata = Encoding.UTF8.GetString(bytes);

                return returndata;
            }
            else
            {

                return returndata;
            }
        }
    }
}
