using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public enum JSONType
    {
        Text,
        Compile,
        Status
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
