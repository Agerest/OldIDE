using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Xml;

namespace Server
{
    static class Server
    {
        private static int PORT = 228;

        public static string CurrentText { get; set; }
        public static List<Client> Users { get; set; }

        static void Main(string[] args)
        {
            Socket server;
            try
            {
                server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                server.Bind(new IPEndPoint(IPAddress.Any, PORT));
                server.Listen(10);
                Console.WriteLine("Сервер создан успешно");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
                return;
            }

            Users = new List<Client>();

            while (true)
            {
                Socket userSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    userSocket = server.Accept();
                    Client user = new Client(userSocket, Users.Count);
                    Console.WriteLine("New user connected. ID = " + Users.Count);
                    Users.Add(user);
                    if (CurrentText != null)
                    {
                        Json json = new Json(JSONType.text, CurrentText);
                        string message = JsonConvert.SerializeObject(json);
                        user.SendMessage(message);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public static void SendMessageAllUser(string message, Client user)
        {
            try
            {
                foreach (Client u in Users) if (!u.Equals(user)) u.SendMessage(message);
                Console.WriteLine("Отправка сообщения всем пользователям успешна");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
