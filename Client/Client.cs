using Newtonsoft.Json;
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
        private const string ONLINE_STATUS = "Online";
        private const string OFFILE_STATUS = "Offline";
        private const int PORT = 228;

        private static string IP;
        private static Socket Socket;
        private static NetworkStream Stream;
        private static BinaryReader Reader;
        private static BinaryWriter Writer;
        public static bool Connected = false;
        private static TextBox TextBox { get; set; }
        private static Label Status { get; set; }

        public static void SetProperty(string ip, TextBox textBox, Label label)
        {
            IP = ip;
            Status = label;
            TextBox = textBox;
        }
        
        public static void Connect()
        {
            try
            {
                Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Socket.Connect(new IPEndPoint(IPAddress.Parse(IP), PORT));
                Stream = new NetworkStream(Socket);
                Reader = new BinaryReader(Stream);
                Writer = new BinaryWriter(Stream);
                Connected = true;
                WriteToStatusLabel(ONLINE_STATUS);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            Thread thread = new Thread(Working);
            thread.Start();
        }

        private static void Working()
        {
            while (true)
            {
                {
                    try
                    {
                        string message = ReceiveMessage();
                        JSON json = JsonConvert.DeserializeObject<JSON>(message);
                        JsonParse(json);
                    }
                    catch
                    {
                        return;
                    }
                }
            }
        }

        private static void WriteToStatusLabel(string text)
        {
            Status.Invoke((MethodInvoker)delegate
            {
                Status.Text = text;
            });
        }

        private static void WriteToTextBox(string text)
        {
            TextBox.Invoke((MethodInvoker)delegate
            {
                TextBox.Text = text;
            });
        }

        public static void SendMessage(string message)
        {
            try
            {
                Writer.Write(message);
                Writer.Flush();
            }
            catch
            {
                CloseConnection();
            }
        }

        public static string ReceiveMessage()
        {
            string message = "";
            try
            {
                message = Reader.ReadString();
            }
            catch
            {
                CloseConnection();
            }
            return message;
        }

        private static void JsonParse(JSON json)
        {
            switch (json.Type)
            {
                case JSONType.Text:
                    WriteToTextBox(json.Data);
                    break;
                case JSONType.Compile:
                    MessageBox.Show(json.Data);
                    break;
                case JSONType.Status:
                    if (json.Data == OFFILE_STATUS) CloseConnection();
                    break;
            }
        }

        public static void Action(string text, JSONType type)
        {
            Action(text, null, type);
        }

        public static void Action(string text1, string text2, JSONType type)
        {
            JSON json = new JSON(type, text1, text2);
            string j = JsonConvert.SerializeObject(json);
            SendMessage(j);
        }

        public static void CloseApplication()
        {
            Action(OFFILE_STATUS, JSONType.Status);
        }

        private static void CloseConnection()
        {
            try
            {
                Socket.Close();
                Stream.Close();
                Reader.Close();
                Writer.Close();
                WriteToStatusLabel(OFFILE_STATUS);
            }
            catch
            {
                return;
            }
        }
    }
}


