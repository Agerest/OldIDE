namespace Server
{
    public enum JSONType
    {
        text,
        compile,
        status
    }

    class Json
    {
        public JSONType Type { set;  get; }
        public string Data { set; get; }

        public Json(JSONType type, string data)
        {
            Type = type;
            Data = data;
        }
    }
}
