using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Server
{
    static class Compiler
    {
        private const string XMLPATH = @"C:\Users\Agerest\source\repos\Shorin1\IDE\Server\main.xml";
        private static string JavacPath;
        private static string JavaPath;
        private static string WorkFolderPath;

        private static void ReadXML()
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(XMLPATH);
            XmlElement xRoot = xDoc.DocumentElement;
            foreach (XmlNode xnode in xRoot)
            {
                switch (xnode.Name)
                {
                    case "compile":
                        JavacPath = xnode.InnerText;
                        break;
                    case "run":
                        JavaPath = xnode.InnerText;
                        break;
                    case "workFolder":
                        WorkFolderPath = xnode.InnerText;
                        break;
                }
            }
        }
        public static string Compile(string program, string nameClass)
        {
            ReadXML();
            StreamWriter sw = new StreamWriter(nameClass + ".java", false, System.Text.Encoding.Default);
            sw.WriteLine(program);
            sw.Close();
            RunCmd(ToQuotes(JavacPath) + " " + ToQuotes(WorkFolderPath + @"\" + nameClass + ".java"));
            return RunCmd(ToQuotes(JavaPath) + " -classpath " + WorkFolderPath + " " + nameClass);
        }
        private static string ToQuotes(string str)
        {
            return "\"" + str + "\"";
        }
        private static string RunCmd(string commands)
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
