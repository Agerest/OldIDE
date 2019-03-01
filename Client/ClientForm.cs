using Newtonsoft.Json;
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
                JSON json = JsonConvert.DeserializeObject<JSON>(message);
                switch (json.Type)
                {
                    case JSONType.text:
                        WriteTextToTextBox(json.Data);
                        break;
                    case JSONType.cmd:
                        MessageBox.Show(json.Data);
                        break;
                }
                
            }
        }

        private void WriteTextToTextBox(string text)
        {
            textBox1.Invoke((MethodInvoker)delegate
            {
                textBox1.Text = text;
            });
        }

        private void button2_Click(object sender, EventArgs e)
        {
            JSON json = new JSON(JSONType.compile, textBox1.Text);
            string j = JsonConvert.SerializeObject(json);
            Client.SendMessage(j);
        }

        private void KeyPressTextBox1(object sender, KeyPressEventArgs e)
        {
            JSON json = new JSON(JSONType.text, textBox1.Text);
            string j = JsonConvert.SerializeObject(json);
            Client.SendMessage(j);
        }
    }
}
