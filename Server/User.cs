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
        private NetworkStream stream;
        private BinaryReader reader;
        private BinaryWriter writer;
        private bool connected = false;
        public int UserID { get; }
        private Thread thread;

        public User(Socket user, int userID)
        {
            Socket = user;
            UserID = userID;
            stream = new NetworkStream(user);
            reader = new BinaryReader(stream);
            writer = new BinaryWriter(stream);
            thread = new Thread(Working);
            thread.Start();
            connected = true;
        }

        private void Working()
        {
            while (connected)
            {
                string message = ReceiveMessage();
                JsonParse(message);
            }
        }

        public void SendMessage(string message)
        {
            try
            {
                writer.Write(message);
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
                message = reader.ReadString();
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
                        string message = Server.Compile(json.Data);
                        json = new JSON(JSONType.cmd, message);
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
            connected = false;
            reader.Close();
            writer.Close();
            stream.Close();
            Socket.Shutdown(SocketShutdown.Both);
            Socket.Close();
            Console.WriteLine("Connection closed");
        }
    }
}
