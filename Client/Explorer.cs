using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    static class Explorer
    {
        private static string name;
        private static string path;
        private static TreeView treeView;

        public static void SetProjectPath(string projectName, string projectPath, TreeView tree)
        {
            name = projectName;
            path = projectPath;
            treeView = tree;
            CreateFileNodes();
        }

        private static void CreateFileNodes()
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
