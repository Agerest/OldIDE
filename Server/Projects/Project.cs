using Newtonsoft.Json;
using Server.JSON;
using System.Collections.Generic;
using System.Xml;

namespace Server
{
    class Project
    {
        private const string XML_FILE_NAME = @"\Struct.xml";

        private List<ProjectStruct.File> files;
        public string Name { get; set; }
        public int ID { get; set; }
        private XmlDocument xml;

        public Project(int ID, string name, string projectPath)
        {
            this.ID = ID;
            Name = name;
            files = new List<ProjectStruct.File>();
            xml = new XmlDocument();
            xml.Load(projectPath + XML_FILE_NAME);
        }

        private void ReadXml() //Реализовать
        {

        }

        public void AddFiles(ProjectStruct.File _files)
        {
            files.Add(_files);
        }

        public string Serialize()
        {
            List<JsonClass> jsonClasses = new List<JsonClass>();
            foreach (ProjectStruct.File file in files)
            {
                jsonClasses.Add(file.ToJsonClass());
            }
            JsonProject jsonProject = new JsonProject(ID, jsonClasses.ToArray(), xml);
            return JsonConvert.SerializeObject(jsonProject);
        }
    }
}
