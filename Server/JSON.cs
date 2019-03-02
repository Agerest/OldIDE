using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public enum JSONType
    {
        text,
        compile,
        status
    }

    class JSON
    {
        public JSONType Type { set;  get; }
        public string Data { set; get; }
        public string Data2 { set; get; }

        public JSON(JSONType type, string data, string data2)
        {
            Type = type;
            Data = data;
            Data2 = data2;
        }
    }
}
