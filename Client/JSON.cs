using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public enum JSONType
    {
        text,
        compile,
        cmd,
        status
    }

    class JSON
    {
        public JSONType Type { set;  get; }
        public string Data { set; get; }

        public JSON(JSONType type, string data)
        {
            Type = type;
            Data = data;
        }
    }
}
