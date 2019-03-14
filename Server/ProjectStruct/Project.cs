using Newtonsoft.Json;
using Server.JSON;
using Server.ProjectStruct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Project
    {
        private List<Class> classes;
        private string Name { get; set; }
        private int ID { get; set; }

        public Project(string name, int ID)
        {
            Name = name;
            this.ID = ID;
            classes = new List<Class>();
        }

        public void AddClass(Class _class)
        {
            classes.Add(_class);
        }

        public string Serialize()
        {
            List<JsonClass> jsonClasses = new List<JsonClass>();
            foreach (Class file in classes)
            {
                jsonClasses.Add(file.ToJsonClass());
            }
            JsonProject jsonProject = new JsonProject(this.ID, jsonClasses.ToArray());
            return JsonConvert.SerializeObject(jsonProject);
        } 

    }
}
