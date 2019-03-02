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
    class Client
    {
        private const string ONLINE_STATUS = "Online";
        private const string OFFILE_STATUS = "Offline";
        private const int PORT = 228;

        private string IP;
        private Socket Socket;
        private NetworkStream Stream;
        private BinaryReader Reader;
        private BinaryWriter Writer;
        public bool Connected = false;
        private TextBox TextBox { get; set; }
        private Label Status { get; set; }

        public void SetProperty(string ip, TextBox textBox, Label label)
        {
            IP = ip;
            Status = label;
            TextBox = textBox;
        }
        
        public void Connect()
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

        private void Working()
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

        private void WriteToStatusLabel(string text)
        {
            Status.Invoke((MethodInvoker)delegate
            {
                Status.Text = text;
            });
        }

        private void WriteToTextBox(string text)
        {
            TextBox.Invoke((MethodInvoker)delegate
            {
                TextBox.Text = text;
            });
        }

        public void SendMessage(string message)
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

        public string ReceiveMessage()
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

        private void JsonParse(JSON json)
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

        public void Action(string text, JSONType type)
        {
            JSON json = new JSON(type, text, null);
            string j = JsonConvert.SerializeObject(json);
            SendMessage(j);
        }

        public void Action(string text1, string text2, JSONType type)
        {
            JSON json = new JSON(type, text1, text2);
            string j = JsonConvert.SerializeObject(json);
            SendMessage(j);
        }

        public void CloseApplication()
        {
            Action(OFFILE_STATUS, JSONType.Status);
        }

        private void CloseConnection()
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


