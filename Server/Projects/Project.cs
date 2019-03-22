using Newtonsoft.Json;
using Server.JSON;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace Server
{
    class Project
    {
        private const string XML_FILE_NAME = @"\Struct.xml";

        public int ID { get; set; }
        public string Name { get; set; }
        private string projectPath;
        private List<FileInfo> files;
        
        public Project(int ID, string name, string projectPath)
        {
            this.ID = ID;
            Name = name;
            files = new List<FileInfo>();
            this.projectPath = projectPath;
            DirectoryInfo directory = new DirectoryInfo(projectPath);
            ReadFiles(directory);
        }

        private void ReadFiles(DirectoryInfo directory) 
        {
            FileInfo[] files = directory.GetFiles();

            foreach (FileInfo f in files)
            {
                this.files.Add(f);
            }
            DirectoryInfo[] directories = directory.GetDirectories();

            foreach(DirectoryInfo di in directories)
            {
                ReadFiles(di);
            }
        }

        private void readXml()
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(projectPath);

        }

        public string GetXmlFile()
        {
            return File.ReadAllText(projectPath + XML_FILE_NAME);
        }

        /*public void AddFiles(FileInfo _files)
        {
            files.Add(_files);
        }*/
    }
}
