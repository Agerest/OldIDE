using Client.JSON;
using Newtonsoft.Json;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace Client
{
    static class Explorer
    {
        private const string XML_PATH = @"Project\Struct.xml";

        private static TreeNode rNode;
        private static TreeView tView;

        public static void OpenProject(long projectID, TreeView treeView)
        {
            Client.Action(JsonType.OpenProject, projectID.ToString());
            tView = treeView;
        }

        public static void LoadProject(string xmlFileSerialize)
        {
            string xmlFile = JsonConvert.DeserializeObject<string>(xmlFileSerialize);
            File.WriteAllText(XML_PATH, xmlFile);
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(XML_PATH);
            XmlElement xroot = xDoc.DocumentElement;
            rNode = new TreeNode(xroot.GetAttribute("name"));
            FillTreeView(xroot, rNode);
            WriteToTreeView();
        }

        private static void FillTreeView(XmlElement node, TreeNode parentNode)
        {
            string name = node.GetAttribute("name");
            TreeNode treeNode = new TreeNode(name);
            parentNode.Nodes.Add(treeNode);

            foreach (XmlElement xelem in node.ChildNodes)
            {
                if (xelem.Name == "File")
                {
                    treeNode.Nodes.Add(xelem.GetAttribute("name"));
                }
                if (xelem.Name == "Folder")
                {
                    FillTreeView(xelem, treeNode);
                }
            }
        }

        private static void WriteToTreeView()
        {
            tView.Invoke((MethodInvoker)delegate
            {
                tView.Nodes.Add(rNode);
            });
        }
    }
}
