using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class ClientForm : Form
    {
        public ClientForm()
        {
            InitializeComponent();
        }

        private void MainTextBoxChanged(object sender, EventArgs e)
        {
            Client.SendMessage(textBox1.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Client.Connect(textBox2.Text);
            Thread thread = new Thread(Working);
            thread.Start();
        }

        private void Working()
        {
            while (true)
            {
                string message = Client.ReceiveMessage();
                textBox1.Invoke((MethodInvoker)delegate
                {
                    textBox1.Text = message;
                });
            }
        }
    }
}
