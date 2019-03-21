using Newtonsoft.Json;
using Server.JSON;
using System.Collections.Generic;
using System.IO;

namespace Server
{
    class Project
    {
        private const string XML_PATH = "";
        private List<ProjectStruct.File> classes;
        private string Name { get; set; }
        private int ID { get; set; }

        public Project()
        {
            classes = new List<ProjectStruct.File>();
        }

        public void AddClass(ProjectStruct.File _class)
        {
            classes.Add(_class);
        }

        public string Serialize()
        {
            List<JsonClass> jsonClasses = new List<JsonClass>();
            foreach (ProjectStruct.File file in classes)
            {
                jsonClasses.Add(file.ToJsonClass());
            }
            JsonProject jsonProject = new JsonProject(this.ID, jsonClasses.ToArray());
            return JsonConvert.SerializeObject(jsonProject);
        }

        public void OpenProject()
        {

        }

        

    }
}
