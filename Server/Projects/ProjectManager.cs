using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Server.Projects
{
    static class ProjectManager
    {
        private const string XML_FILE_PATH = @"Properties\ProjectManager.xml";

        private static List<Project> projects;
        private static XmlDocument xml;

        public static void ReadXml()
        {
            xml = new XmlDocument();
            projects = new List<Project>();
            xml.Load(XML_FILE_PATH);
            XmlElement nodeList = xml.DocumentElement;
            foreach (XmlNode xnode in nodeList)
            {
                int ID = int.Parse(xnode["ID"].InnerText);
                string name = xnode["Name"].InnerText;
                string projectPath = xnode["ProjectPath"].InnerText;
                projects.Add(new Project(ID, name, projectPath));
            }
        }

        

        public static Project GetProject(long ID)
        {
            foreach (Project project in projects)
            {
                if (project.ID == ID)
                {
                    return project;
                }
            }
            return null;
        }


    }
}
