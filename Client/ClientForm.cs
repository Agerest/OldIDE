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
                Client.SetProperty("127.0.0.1", codeTextBox, statusLabel);
                Client.Connect();
            }
        }

        private void compileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Client.Action(codeTextBox.Text, projectName, JSONType.Compile);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Explorer.OpenProject(projectNameTextBox.Text, explorerTreeView);
        }

        private void keyUpCodeTextBox(object sender, KeyEventArgs e)
        {
            if (Client.Connected)
            {
                Client.Action(codeTextBox.Text, JSONType.Text);
            }
        }

        private void exit(object sender, FormClosingEventArgs e)
        {
            Client.CloseApplication();
        }
        
    }
}
