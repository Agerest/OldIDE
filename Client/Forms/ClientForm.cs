using Client.JSON;
using System;
using System.Windows.Forms;

namespace Client
{
    public partial class clientForm : Form
    {
        private const string ONLINE_STATUS = "Online";
        private const string OFFILE_STATUS = "Offline";

        public clientForm()
        {
            InitializeComponent();
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Client.Connected)
            {
                Client.SetProperty("127.0.0.1", codeTextBox, resultTextBox, statusLabel);
                Client.Connect();
            }
        }

        private void compileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Client.Action(JsonType.Compile, codeTextBox.Text);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Explorer.OpenProject(0, explorerTreeView);
        }

        private void keyUpCodeTextBox(object sender, KeyEventArgs e)
        {
            if (Client.Connected)
            {
                Client.Action(JsonType.Text, codeTextBox.Text);
            }
        }

        private void exit(object sender, FormClosingEventArgs e)
        {
            Client.Action(JsonType.Status, OFFILE_STATUS);
        }
        
    }
}
