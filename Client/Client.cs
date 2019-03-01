using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    static class Client
    {
        private static Socket client;
        private static NetworkStream stream;
        private static BinaryReader reader;
        private static BinaryWriter writer;
        private static bool connected = false;
        private static int port = 228;

        public static void Connect(string ip)
        {
            try
            {
                client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                client.Connect(new IPEndPoint(IPAddress.Parse(ip), port));
                stream = new NetworkStream(client);
                reader = new BinaryReader(stream);
                writer = new BinaryWriter(stream);
                connected = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                client.Close();
            }
            finally
            {
                MessageBox.Show("Подключение успешно");
            }
        }

        public static bool Connected()
        {
            return connected;
        }

        public static void SendMessage(string message)
        {
            /*byte[] buffer = Encoding.Unicode.GetBytes(message);

            try
            {
                client.Send(buffer);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                connected = false;
                client.Close();
            }*/

            writer.Write(message);

        }

        public static void Compile(string program)
        {
            string writePath = "test";
            StreamWriter sw = new StreamWriter(writePath, false, System.Text.Encoding.Default);
            sw.WriteLine(program);
            sw.Close();
            runCmd(@"""C:\Program Files\Java\jdk-11.0.2\bin\javac.exe""" + @"""D:\Projects\C#\IDE\Client\bin\Debug\" + writePath + @".java""");
            runCmd(@"""C:\Program Files\Java\jdk-11.0.2\bin\java.exe""" + @" -classpath D:\Projects\C#\IDE\Client\bin\Debug\ " + writePath);
        }

        private static void runCmd(string commands)
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
            MessageBox.Show(sb.ToString());
        }

        public static string ReceiveMessage()
        {
            string message = reader.ReadString();
            return message;
        }


    }
}
