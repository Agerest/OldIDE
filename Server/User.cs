using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class User
    {
        private Socket user;
        private NetworkStream stream;
        private BinaryReader reader;
        private BinaryWriter writer;

        public User(Socket user)
        {
            this.user = user;
            stream = new NetworkStream(user);
            reader = new BinaryReader(stream);
            writer = new BinaryWriter(stream);
            Thread thread = new Thread(Working);
            thread.Start();
        }

        private void Working()
        {
            while (true)
            {
                string message = ReceiveMessage();
                Console.WriteLine(message);
                Server.SendMessageAllUser(message, this);
            }
        }

        public void SendMessage(string message)
        {
            byte[] buffer = Encoding.Unicode.GetBytes(message);

            try
            {
                user.Send(buffer);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                user.Close();
            }
            finally
            {
                Console.WriteLine("Отправка сообщения успешна");
            }
        }

        private string ReceiveMessage()
        {
            StringBuilder SB = new StringBuilder();
            int bytes = 0;
            byte[] buffer = new byte[256];

            try
            {
                do
                {
                    bytes = user.Receive(buffer);
                    SB.Append(Encoding.Unicode.GetString(buffer, 0, bytes));
                }
                while (user.Available > 0);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return SB.ToString();
        }
    }
}
