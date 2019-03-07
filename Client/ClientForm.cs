using System;
using System.Windows.Forms;

namespace Client
{
    public partial class clientForm : Form
    {
        private Client client = new Client();
        private string projectName;

        public clientForm()
        {
            InitializeComponent();
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!client.Connected)
            {
                client.SetProperty("127.0.0.1", CodeTextBox, StatusLabel);
                client.Connect();
            }
        }

        private void compileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            client.Action(CodeTextBox.Text, projectName, JSONType.Compile);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void keyUpCodeTextBox(object sender, KeyEventArgs e)
        {
            if (client != null && client.Connected) client.Action(CodeTextBox.Text,JSONType.Text);
        }

        private void exit(object sender, FormClosingEventArgs e)
        {
            client.CloseApplication();
        }
        
    }
}
