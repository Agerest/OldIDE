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
            client.action(codeTextBox.Text, textBox1.Text, JSONType.compile);
        }

        private void KeyUpCodeTextBox(object sender, KeyEventArgs e)
        {
            if (client != null && client.Connected) client.action(codeTextBox.Text,JSONType.text);
        }

        private void Exit(object sender, FormClosingEventArgs e)
        {
            client.CloseApplication();
        }

    }
}
