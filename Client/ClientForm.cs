using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace Client
{
    public partial class ClientForm : Form
    {
        private Client client = new Client();
        public ClientForm()
        {
            InitializeComponent();
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            if (!client.Connected)
            {
                client.SetProperty(ipTextBox.Text, codeTextBox, statusLabel);
                client.Connect();
            }
        }

        private void compileButton_Click(object sender, EventArgs e)
        {
            client.Compile(codeTextBox.Text);
        }

        private void KeyUpCodeTextBox(object sender, KeyEventArgs e)
        {
            if (client != null && client.Connected) client.SendCodeText(codeTextBox.Text);
        }

        private void Exit(object sender, FormClosingEventArgs e)
        {
            client.CloseApplication();
        }



        class Client
        {
            private const string ONLINE_STATUS = "Online";
            private const string OFFILE_STATUS = "Offline";

            private string ip;
            private Socket socket;
            private NetworkStream stream;
            private BinaryReader reader;
            private BinaryWriter writer;
            public bool Connected = false;
            private TextBox textBox { get;  set; }
            private Label Status { get;  set; }

            public void SetProperty(string ip, TextBox textBox, Label label)
            {
                this.ip = ip;
                Status = label;
                this.textBox = textBox;
            }

            public void Connect()
            {
                int port = 228;
                try
                {
                    socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    socket.Connect(new IPEndPoint(IPAddress.Parse(ip), port));
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
                            WriteToTextBox(json.Data);
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
                    case JSONType.cmd:
                        MessageBox.Show(json.Data);
                        break;
                    case JSONType.status:
                        if (json.Data == OFFILE_STATUS) CloseConnection();
                        break;
                }
            }

            public void Compile(string text)
            {
                JSON json = new JSON(JSONType.compile, text);
                string j = JsonConvert.SerializeObject(json);
                SendMessage(j);
            }

            public void SendStatus(string status)
            {
                JSON json = new JSON(JSONType.status, status);
                string message = JsonConvert.SerializeObject(json);
                SendMessage(message);
            }

            public void SendCodeText(string text)
            {
                JSON json = new JSON(JSONType.text, text);
                string j = JsonConvert.SerializeObject(json);
                SendMessage(j);
            }

            public void CloseApplication()
            {
                SendStatus(OFFILE_STATUS);
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
}
