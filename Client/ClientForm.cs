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
            client = new Client(ipTextBox.Text, codeTextBox);
            if (client.Connected())
            {
                statusLabel.Text = "Online";
            }
        }

        private void compileButton_Click(object sender, EventArgs e)
        {
            client.ButtonCompiteClickEvent(codeTextBox.Text);
        }

        private void KeyDownCodeTextBox(object sender, KeyEventArgs e)
        {
            client.KeyDownEvent(codeTextBox.Text);
        }


        class Client
        {
            private Socket client;
            private NetworkStream stream;
            private BinaryReader reader;
            private BinaryWriter writer;
            private bool connected = false;
            private TextBox textBox;

            public Client(string ip, TextBox textBox)
            {
                int port = 228;
                Connect(ip, port);
                this.textBox = textBox;
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
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    stream.Close();
                    reader.Close();
                    writer.Close();
                    client.Close();
                    return;
                }

                Thread thread = new Thread(Working);
                thread.Start();
            }

            private void Working()
            {
                while (true)
                {
                    string message = ReceiveMessage();
                    JSON json = JsonConvert.DeserializeObject<JSON>(message);
                    WriteToTextBox(json.Data);
                    JsonParse(json);
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
                writer.Write(message);
            }

            public string ReceiveMessage()
            {
                string message = "";
                try
                {
                    message = reader.ReadString();
                }
                catch (Exception ex)
                {
                    CloseConnection();
                }
                return message;
            }

            public void ButtonCompiteClickEvent(string text)
            {
                JSON json = new JSON(JSONType.compile, text);
                string j = JsonConvert.SerializeObject(json);
                SendMessage(j);
            }

            public void KeyDownEvent(string text)
            {
                JSON json = new JSON(JSONType.text, text);
                string j = JsonConvert.SerializeObject(json);
                SendMessage(j);
            }

            private void CloseConnection()
            {
                client.Close();
                stream.Close();
                reader.Close();
                writer.Close();
            }
        }

        
    }
}
