using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using Client.JSON;

namespace Client
{
    static class Client
    {
        private const string ONLINE_STATUS = "Online";
        private const string OFFILE_STATUS = "Offline";
        private const int PORT = 228;

        private static string IP;
        private static Socket socket;
        private static NetworkStream stream;
        private static BinaryReader reader;
        private static BinaryWriter writer;

        public static bool Connected { get; set; } = false;

        public static void SetProperty(string ip, TextBox code, TextBox result, Label status)
        {
            IP = ip;
            JsonParser.SetProperties(code, result, status);
        }
        
        public static void Connect()
        {
            try
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(new IPEndPoint(IPAddress.Parse(IP), PORT));
                stream = new NetworkStream(socket);
                reader = new BinaryReader(stream);
                writer = new BinaryWriter(stream);
                Connected = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Connect: " + ex.Message);
                return;
            }

            Thread thread = new Thread(Working);
            thread.Start();
        }

        private static void Working()
        {
            while (Connected)
            {
                try
                {
                    string message = ReceiveMessage();
                    Json json = JsonConvert.DeserializeObject<Json>(message);
                    JsonParser.Parse(json);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Working: " + ex.Message);
                    return;
                }
            }
        }


        public static void SendMessage(string message)
        {
            try
            {
                writer.Write(message);
                writer.Flush();
            }
            catch (Exception ex)
            {
                MessageBox.Show("SendMessage: " + ex.Message);
                CloseConnection();
            }
        }

        public static string ReceiveMessage()
        {
            string message = "";
            try
            {
                message = reader.ReadString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("ReceiveMessage: " + ex.Message);
                CloseConnection();
            }
            return message;
        }

        public static void Action(JsonType type, string text)
        {
            Json json = new Json(type, text);
            string j = JsonConvert.SerializeObject(json);
            SendMessage(j);
        }

        public static void CloseConnection()
        {
            try
            {
                socket.Close();
                stream.Close();
                reader.Close();
                writer.Close();
            }
            catch
            {
                return;
            }
        }
    }
}


