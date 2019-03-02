using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace Server
{
    class User
    {
        private const string ONLINE_STATUS = "Online";
        private const string OFFILE_STATUS = "Offline";

        public Socket Socket { get; }
        private NetworkStream Stream;
        private BinaryReader Reader;
        private BinaryWriter Writer;
        private bool Connected = false;
        public int UserID { get; }
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
                JsonParse(message);
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

        private void JsonParse(string jsonSerialize)
        {
            JSON json = JsonConvert.DeserializeObject<JSON>(jsonSerialize);

            try
            {
                switch (json.Type)
                {
                    case JSONType.text:
                        Server.CurrentText = json.Data;
                        Server.SendMessageAllUser(jsonSerialize, this);
                        break;
                    case JSONType.compile:
                        string message = Compiler.Compile(json.Data, json.Data2);
                        json = new JSON(JSONType.compile, message, null);
                        string j = JsonConvert.SerializeObject(json);
                        SendMessage(j);
                        break;
                    case JSONType.status:
                        if (json.Data == OFFILE_STATUS) CloseConnection();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public override bool Equals(object obj)
        {
            return ((User)obj).Socket == Socket;
        }

        private void CloseConnection()
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
