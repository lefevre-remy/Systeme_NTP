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
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur socket : "+e.Message);
            }
            
            Console.ReadKey(); //pause ecran
        }
    }
}
