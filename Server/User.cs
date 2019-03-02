using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace Server
{
    class User
    {
        private Socket user;
        private NetworkStream stream;
        private BinaryReader reader;
        private BinaryWriter writer;

        public int UserID { get; }

        public User(Socket user, int userID)
        {
            this.user = user;
            UserID = userID;
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
            string message = "";
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

            Server.CurrentText = message;
            return message;
        }

        private void JsonParse(string jsonSerialize)
        {
            JSON json = JsonConvert.DeserializeObject<JSON>(jsonSerialize);
            switch (json.Type)
            {
                case JSONType.text:
                    Server.SendMessageAllUser(jsonSerialize, this);
                    break;
                case JSONType.compile:
                    string message = Server.Compile(json.Data);
                    /*json = new JSON(JSONType.cmd, message);
                    string j = JsonConvert.SerializeObject(json);
                    SendMessage(j);*/
                    break;
                case JSONType.status:
                    if (json.Data == "Offile") CloseConnection();
                    break;
            }
        }

        public override bool Equals(object obj)
        {
            return ((User)obj).UserID == UserID;
        }

        private void CloseConnection()
        {
            Server.Users.Remove(this);
            user.Close();
            stream.Close();
            reader.Close();
            writer.Close();
        }

        private void OnApplicationExit(object sender, EventArgs e)
        {
            CloseConnection();
        }
    }
}
