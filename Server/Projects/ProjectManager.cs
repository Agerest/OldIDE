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
            xml.Load(XML_FILE_PATH);
            XmlElement nodeList = xml.DocumentElement;
            foreach (var v in nodeList)
            {

            }
            int ID ;
            string name;
            string xmlFilePath;
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
