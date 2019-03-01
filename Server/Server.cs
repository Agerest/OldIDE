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
    class Server
    {
        public static string javacPath;
        public static string javaPath;
        public static string workFolderPath;
        static List<User> users;
        static int port = 228;
        static string currentText;

        public static string CurrentText { get => currentText; set => currentText = value; }

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
            //readXML();
            while (true)
            {
                Socket userSocket;
                try
                {
                    userSocket = server.Accept();
                    User user = new User(userSocket, users.Count);
                    users.Add(user);
                    user.SendMessage(currentText);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    Console.WriteLine("New user connected");
                }
            }
        }

        public static void SendMessageAllUser(string message, User user)
        {
            try
            {
                foreach (User u in users)
                {
                    if (!u.Equals(user))
                    {
                        u.SendMessage(message);
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

        private static void readXML()
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load("main.xml");
            XmlElement xRoot = xDoc.DocumentElement;
            foreach (XmlNode xnode in xRoot)
            {
                if (xnode.Name == "compile")
                {
                    javacPath = xnode.InnerText;
                }
                if (xnode.Name == "run")
                {
                    javaPath = xnode.InnerText;
                }
                if (xnode.Name == "workFolder")
                {
                    workFolderPath = xnode.InnerText;
                }
            }
        }

        public static string Compile(string program)
        {
            string writePath = "test";
            StreamWriter sw = new StreamWriter(writePath + ".java", false, System.Text.Encoding.Default);
            sw.WriteLine(program);
            sw.Close();
            runCmd(@"""" + javacPath + @""" " + workFolderPath + writePath + @".java""");
            return runCmd(@"""" + javaPath + @""" -classpath " + workFolderPath + @" " + writePath);
        }

        private static string runCmd(string commands)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    RedirectStandardInput = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true
                }
            };
            process.Start();

            using (StreamWriter pWriter = process.StandardInput)
            {
                if (pWriter.BaseStream.CanWrite)
                {
                    foreach (var line in commands.Split('\n'))
                        pWriter.WriteLine(line);
                }
            }
            StreamReader sr = process.StandardOutput;
            StringBuilder sb = new StringBuilder();
            while (!sr.EndOfStream)
            {
                sb.Append(sr.ReadLine()).Append("\n");
            }
            return sb.ToString();
        }
    }
}
