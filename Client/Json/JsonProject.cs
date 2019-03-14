using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.JSON
{
    class JsonProject
    {
        private long ID { get; set; }
        private JsonClass[] Classes { get; set; }

        public JsonProject(long id, JsonClass[] classes)
        {
            ID = id;
            Classes = classes;
        }

    }
}
