using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Server
    {
        static List<User> users;
        static int port = 228;
        static void Main(string[] args)
        {
            Socket server;
            try
            {
                server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                server.Bind(new IPEndPoint(IPAddress.Any, port));
                server.Listen(10);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
                return;
            }
            finally
            {
                Console.WriteLine("Сервер создан успешно");
            }

            users = new List<User>();

            while (true)
            {
                Socket userSocket;
                try
                {
                    userSocket = server.Accept();
                    User user = new User(userSocket);
                    users.Add(user);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    Console.Write("New user connected");
                }
            }
        }

        static void SendMessage(string message, User user)
        {
            user.SendMessage(message);
        }

        public static void SendMessageAllUser(string message, User user)
        {
            try
            {
                foreach (User u in users)
                {
                    if (!u.Equals(user))
                    {
                        SendMessage(message, user);
                    }
                }
            } 
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.WriteLine("Отправка сообщения всем пользователям успешна");
            }
        }
    }
}
