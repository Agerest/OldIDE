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
        public int ID { get; set; }
        public string Name { get; set; }
        private string projectPath;
        private List<FileInfo> files;
        public TreeNode TreeNode { get; set; }
        
        public Project(int ID, string name, string projectPath)
        {
            this.ID = ID;
            Name = name;
            files = new List<FileInfo>();
            this.projectPath = projectPath;
            TreeNode = new TreeNode(Name);
            DirectoryInfo directory = new DirectoryInfo(projectPath);
            ReadFiles(directory, TreeNode);
        }

        private void ReadFiles(DirectoryInfo directory, TreeNode parentNode) 
        {
            TreeNode node = new TreeNode(directory.Name);
            FileInfo[] files = directory.GetFiles();

            foreach (FileInfo f in files)
            {
                this.files.Add(f);
                node.Nodes.Add(f.Name);
            }

            parentNode.Nodes.Add(node);
            DirectoryInfo[] directories = directory.GetDirectories();

            foreach(DirectoryInfo di in directories)
            {
                ReadFiles(di, node);
            }
        }

        /*public void AddFiles(FileInfo _files)
        {
            files.Add(_files);
        }*/
    }
}
