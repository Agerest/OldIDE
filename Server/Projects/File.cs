﻿using Newtonsoft.Json;
using Server.JSON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.ProjectStruct
{
    class File
    {

        private int ID { get; set; }
        private string Name { get; set; }
        private string Text { get; set; }

        public File(int ID, string name, string text)
        {
            this.ID = ID;
            Name = name;
            Text = text;
        }

        public JsonClass ToJsonClass()
        {
            return new JsonClass(this.ID, this.Name, this.Text);
        }

    }
}
