using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Projects
{
    static class ProjectManager
    {
        private const string XML_FILE_PATH = @"Properties\ProjectManager.xml";//Пофиксить

        private static List<Project> projects;

        public static void ReadXml()
        {

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
