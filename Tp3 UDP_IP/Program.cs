using System;
using System.Net;
using System.Net.Sockets;

namespace Client_UDP_IP
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[] trame = new byte[48]; //48 octects = 384 bits
            //COnstruction de la trame à envoyer au serveur ntp
            //champ VN à 4, champ Mode à 3, donc le premier octet
            //+-+-+-+-+-+-+-+-+
            //| LI | VN |Mode |
            //+-+-+-+-+-+-+-+-+
            //+0 0|1 0 0|0 1 1+
            //soit 2 | 3 en hexa
            trame[0] = 0x23;

            //on met l'adresse de destination
            IPAddress ipServeur = IPAddress.Parse("193.49.184.123");

            IPEndPoint Serv = new IPEndPoint(ipServeur, 123);

            try
            {
                //création du socket
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                //on envoie le buffer à l'adresse
                socket.SendTo(trame, Serv);

                EndPoint ServRemote = (EndPoint)Serv;

                //on à récuperer l'heure exacte dans trame
                socket.ReceiveFrom(trame, ref ServRemote);

                //on détruit le socket
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur socket : "+e.Message);
                Console.ReadKey();
            }

            //Mise sous forme ordinateur du nb de sec dans TT
            Array.Reverse(trame, 40, 4);

            //on decode les secondes donc on se place au 40eme octet de notre tableau
            uint nbsec = BitConverter.ToUInt32(trame, 40);

            Console.WriteLine("Nb secondes écoulé depuis 1900 et des brouettes: {0}", nbsec);

            DateTime date = new DateTime(1900, 1, 1);

            date = date.AddSeconds(nbsec);

            date = date.ToLocalTime();

            Console.WriteLine("on est le : {0}", date);
            
            Console.ReadKey(); //pause ecran
        }
    }
}
