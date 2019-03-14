using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.JSON
{
    class JsonClass
    {
        private long ID { get; set; }
        private string Name { get; set; }
        private string Text { get; set; }

        public JsonClass(long id, string name, string text)
        {
            ID = id;
            Name = name;
            Text = text;
        }
    }
}
