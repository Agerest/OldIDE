using System.IO;
using System.Windows.Forms;

namespace Client
{
    static class Explorer
    {
        private static TreeNode rNode;
        private static TreeView tView;

        public static void OpenProject(TreeNode rootNode, TreeView treeView)
        {
            tView = treeView;
            rNode = rootNode;
            treeView.Nodes.Add(rNode);
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
