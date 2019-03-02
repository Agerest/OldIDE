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
        private Client client;
        public ClientForm()
        {
            InitializeComponent();
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            client = new Client(ipTextBox.Text, codeTextBox, statusLabel);
        }

        private void compileButton_Click(object sender, EventArgs e)
        {
            client.Compile(codeTextBox.Text);
        }

        private void KeyUpCodeTextBox(object sender, KeyEventArgs e)
        {
            if (client != null && client.Connected()) client.SendCodeText(codeTextBox.Text);
        }


        class Client
        {
            private const string ONLINE_STATUS = "Online";
            private const string OFFILE_STATUS = "Offline";

            private Socket client;
            private NetworkStream stream;
            private BinaryReader reader;
            private BinaryWriter writer;
            private bool connected = false;
            private TextBox textBox;
            private Label status;

            public Client(string ip, TextBox textBox, Label label)
            {
                status = label;
                this.textBox = textBox;
                int port = 228;
                Connect(ip, port);
            }

            private void Connect(string ip, int port)
            {
                try
                {
                    client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    client.Connect(new IPEndPoint(IPAddress.Parse(ip), port));
                    stream = new NetworkStream(client);
                    reader = new BinaryReader(stream);
                    writer = new BinaryWriter(stream);
                    connected = true;
                    WriteToStatusLabel(ONLINE_STATUS);
                }
                catch
                {
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
                            CloseConnection();
                            return;
                        }
                    }
                }
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
                }
            }

            private void WriteToStatusLabel(string text)
            {
                status.Invoke((MethodInvoker)delegate
                {
                    status.Text = text;
                });
            }

            private void WriteToTextBox(string text)
            {
                textBox.Invoke((MethodInvoker)delegate
                {
                    textBox.Text = text;
                });
            }

            public bool Connected()
            {
                return connected;
            }

            public void SendMessage(string message)
            {
                try
                {
                    writer.Write(message);
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

            public void Compile(string text)
            {
                JSON json = new JSON(JSONType.compile, text);
                string j = JsonConvert.SerializeObject(json);
                SendMessage(j);
            }

            public void SendCodeText(string text)
            {
                JSON json = new JSON(JSONType.text, text);
                string j = JsonConvert.SerializeObject(json);
                SendMessage(j);
            }

            private void CloseConnection()
            {
                connected = false;
                client.Close();
                stream.Close();
                reader.Close();
                writer.Close();
                WriteToStatusLabel(OFFILE_STATUS);
            }
        }
    }
}
