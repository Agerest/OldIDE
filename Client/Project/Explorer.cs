using Client.JSON;
using Newtonsoft.Json;
using System.Windows.Forms;

namespace Client
{
    static class Explorer
    {
        private static TreeNode rNode;
        private static TreeView tView;

        public static void OpenProject(long projectID, TreeView treeView)
        {
            Client.Action(JsonType.OpenProject, projectID.ToString());
            tView = treeView;
        }

        public static void FillTreeView(string treeNodeSerialize)
        {
            rNode = JsonConvert.DeserializeObject<TreeNode>(treeNodeSerialize);
            tView.Nodes.Add(rNode);
        }
    }
}
