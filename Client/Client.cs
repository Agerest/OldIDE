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

        private string ip;
        private Socket socket;
        private NetworkStream stream;
        private BinaryReader reader;
        private BinaryWriter writer;
        public bool Connected = false;
        private TextBox textBox { get; set; }
        private Label Status { get; set; }

        public void SetProperty(string ip, TextBox textBox, Label label)
        {
            this.ip = ip;
            Status = label;
            this.textBox = textBox;
        }
        
        public void Connect()
        {
            try
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(new IPEndPoint(IPAddress.Parse(ip), PORT));
                stream = new NetworkStream(socket);
                reader = new BinaryReader(stream);
                writer = new BinaryWriter(stream);
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
            textBox.Invoke((MethodInvoker)delegate
            {
                textBox.Text = text;
            });
        }

        public void SendMessage(string message)
        {
            try
            {
                writer.Write(message);
                writer.Flush();
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
                message = reader.ReadString();
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
                case JSONType.text:
                    WriteToTextBox(json.Data);
                    break;
                case JSONType.compile:
                    MessageBox.Show(json.Data);
                    break;
                case JSONType.status:
                    if (json.Data == OFFILE_STATUS) CloseConnection();
                    break;
            }
        }

        public void action(string text, JSONType type)
        {
            JSON json = new JSON(type, text, null);
            string j = JsonConvert.SerializeObject(json);
            SendMessage(j);
        }

        public void action(string text1, string text2, JSONType type)
        {
            JSON json = new JSON(type, text1, text2);
            string j = JsonConvert.SerializeObject(json);
            SendMessage(j);
        }

        public void CloseApplication()
        {
            action(OFFILE_STATUS, JSONType.status);
        }

        private void CloseConnection()
        {
            try
            {
                socket.Close();
                stream.Close();
                reader.Close();
                writer.Close();
                WriteToStatusLabel(OFFILE_STATUS);
            }
            catch
            {
                return;
            }
        }
    }
}


