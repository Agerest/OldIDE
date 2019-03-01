using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    static class Client
    {
        private static Socket client;
        private static NetworkStream stream;
        private static BinaryReader reader;
        private static BinaryWriter writer;
        private static bool connected = false;
        private static int port = 228;

        public static void Connect(string ip)
        {
            try
            {
                client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                client.Connect(new IPEndPoint(IPAddress.Parse(ip), port));
                stream = new NetworkStream(client);
                reader = new BinaryReader(stream);
                writer = new BinaryWriter(stream);
                connected = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                client.Close();
            }
            finally
            {
                MessageBox.Show("Подключение успешно");
            }
        }

        public static bool Connected()
        {
            return connected;
        }

        public static void SendMessage(string message)
        {
            /*byte[] buffer = Encoding.Unicode.GetBytes(message);

            try
            {
                client.Send(buffer);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                connected = false;
                client.Close();
            }*/

            writer.Write(message);

        }

        public static string ReceiveMessage()
        {
            /*StringBuilder SB = new StringBuilder();
            int bytes = 0;
            byte[] buffer = new byte[256];

            try
            {
                do
                {
                    bytes = client.Receive(buffer);
                    SB.Append(Encoding.Unicode.GetString(buffer));
                }
                while (client.Available > 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                client.Close();
            }

            return SB.ToString();*/
            string message = reader.ReadString();
            return message;
        }


    }
}
