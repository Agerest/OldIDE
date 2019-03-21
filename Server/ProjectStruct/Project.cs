using Newtonsoft.Json;
using Server.JSON;
using Server.ProjectStruct;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Project
    {
        private List<File> classes;
        private string Name { get; set; }
        private int ID { get; set; }

        public Project()
        {
            classes = new List<File>();
        }

        public void AddClass(File _class)
        {
            classes.Add(_class);
        }

        public string Serialize()
        {
            List<JsonClass> jsonClasses = new List<JsonClass>();
            foreach (File file in classes)
            {
                jsonClasses.Add(file.ToJsonClass());
            }
            JsonProject jsonProject = new JsonProject(this.ID, jsonClasses.ToArray());
            return JsonConvert.SerializeObject(jsonProject);
        }

        private static void CreateFileNodes()// Нужно перенести на сервер
        {
            DirectoryInfo rootDirectory = new DirectoryInfo(path);
            TreeNode rootNode = new TreeNode(name);
            FillNodes(rootDirectory, rootNode);
            treeView.Nodes.Add(rootNode);
        }

        private static void FillNodes(DirectoryInfo DI, TreeNode parentNode)
        {
            TreeNode node = new TreeNode(DI.Name);
            FileInfo[] files = DI.GetFiles();

            foreach (FileInfo file in files)
            {
                node.Nodes.Add(file.Name);
            }

            parentNode.Nodes.Add(node);

            DirectoryInfo[] directories = DI.GetDirectories();

            foreach (DirectoryInfo directory in directories)
            {
                FillNodes(directory, node);
            }


        }

    }
}
