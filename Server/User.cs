using Newtonsoft.Json;
using Server.JSON;
using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace Server
{
    class User
    {

        public Socket Socket { get; set; }
        private NetworkStream Stream;
        private BinaryReader Reader;
        private BinaryWriter Writer;
        private bool Connected = false;
        public int UserID { get; set; }
        private Thread Thread;

        public User(Socket user, int userID)
        {
            Socket = user;
            UserID = userID;
            Stream = new NetworkStream(user);
            Reader = new BinaryReader(Stream);
            Writer = new BinaryWriter(Stream);
            Thread = new Thread(Working);
            Thread.Start();
            Connected = true;
        }

        private void Working()
        {
            while (Connected)
            {
                string message = ReceiveMessage();
                JsonParser.JsonParse(message, this);
            }
        }

        public void SendMessage(string message)
        {
            try
            {
                Writer.Write(message);
                Writer.Flush();
                Console.WriteLine("К {0}: {1}", UserID, message);
            }
            catch (Exception ex)
            {
                CloseConnection();
                Console.WriteLine(ex.Message);
            }
        }

        private string ReceiveMessage()
        {
            string message =  "";
            try
            {
                message = Reader.ReadString();
                Console.WriteLine("От {0}: {1}", UserID, message);
            }
            catch (Exception ex)
            {
                CloseConnection();
                Console.WriteLine(ex.Message);
            }

            return message;
        }

        

        public override bool Equals(object obj)
        {
            return ((User)obj).Socket == Socket;
        }

        public void CloseConnection()
        {
            Server.Users.Remove(this);
            Connected = false;
            Reader.Close();
            Writer.Close();
            Stream.Close();
            Socket.Shutdown(SocketShutdown.Both);
            Socket.Close();
            Console.WriteLine("Connection closed");
        }
    }
}
