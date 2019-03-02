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
    class Server
    {
        private static string javacPath;
        private static string javaPath;
        private static string workFolderPath;
        private static int port = 228;

        public static string CurrentText { get; set; }
        public static List<User> Users { get; set; }

        static void Main(string[] args)
        {
            Socket server;
            try
            {
                server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                server.Bind(new IPEndPoint(IPAddress.Any, port));
                server.Listen(10);
                Console.WriteLine("Сервер создан успешно");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
                return;
            }

            Users = new List<User>();

            readXML();

            while (true)
            {
                Socket userSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    userSocket = server.Accept();
                    User user = new User(userSocket, Users.Count);
                    Console.WriteLine("New user connected. ID = " + Users.Count);
                    Users.Add(user);
                    if (CurrentText != null)
                    {
                        JSON json = new JSON(JSONType.text, CurrentText);
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

        public static void SendMessageAllUser(string message, User user)
        {
            try
            {
                foreach (User u in Users) if (!u.Equals(user)) u.SendMessage(message);
                Console.WriteLine("Отправка сообщения всем пользователям успешна");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void readXML()
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(@"D:\Projects\C#\IDE1\Server\main.xml");//не знаю как нормально путь здесь прописать
            XmlElement xRoot = xDoc.DocumentElement;
            foreach (XmlNode xnode in xRoot)
            {
                switch (xnode.Name)
                {
                    case "compile":
                        javacPath = xnode.InnerText;
                        break;
                    case "run":
                        javaPath = xnode.InnerText;
                        break;
                    case "workFolder":
                        workFolderPath = xnode.InnerText;
                        break;
                }
            }
        }

        public static string Compile(string program) //сделал более читабельным
        {
            string writePath = "test";
            StreamWriter sw = new StreamWriter(writePath + ".java", false, System.Text.Encoding.Default);
            sw.WriteLine(program);
            sw.Close();
            runCmd(toQuotes(javacPath) + " " + toQuotes(workFolderPath + writePath + ".java"));
            return runCmd(toQuotes(javaPath) + " -classpath " + workFolderPath + " " + writePath);
        }

        private static string toQuotes (string str) //добавление кавычек
        {
            return "\"" + str + "\"";
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
                    foreach (var line in commands.Split('\n')) pWriter.WriteLine(line);
            }
            StreamReader sr = process.StandardOutput;
            StringBuilder sb = new StringBuilder();
            while (!sr.EndOfStream) sb.Append(sr.ReadLine()).Append("\n");
            return sb.ToString();
        }
    }
}
