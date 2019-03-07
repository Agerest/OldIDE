using System;
using System.Windows.Forms;

namespace Client
{
    public partial class clientForm : Form
    {
        private string projectName;

        public clientForm()
        {
            InitializeComponent();
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Client.Connected)
            {
                Client.SetProperty("127.0.0.1", CodeTextBox, StatusLabel);
                Client.Connect();
            }
        }

        private void compileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Client.Action(CodeTextBox.Text, projectName, JSONType.Compile);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void keyUpCodeTextBox(object sender, KeyEventArgs e)
        {
            if (Client.Connected)
            {
                Client.Action(CodeTextBox.Text, JSONType.Text);
            }
        }

        private void exit(object sender, FormClosingEventArgs e)
        {
            Client.CloseApplication();
        }
        
    }
}
