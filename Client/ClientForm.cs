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
        private Client Client = new Client();

        public ClientForm()
        {
            InitializeComponent();
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            if (!Client.Connected)
            {
                Client.SetProperty(IPTextBox.Text, CodeTextBox, StatusLabel);
                Client.Connect();
            }
        }

        private void СompileButton_Click(object sender, EventArgs e)
        {
            Client.Action(CodeTextBox.Text, ProjectNameTextBox.Text, JSONType.compile);
        }

        private void KeyUpCodeTextBox(object sender, KeyEventArgs e)
        {
            if (Client != null && Client.Connected) Client.Action(CodeTextBox.Text,JSONType.text);
        }

        private void Exit(object sender, FormClosingEventArgs e)
        {
            Client.CloseApplication();
        }

        
    }
}
