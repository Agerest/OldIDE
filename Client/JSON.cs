namespace Client
{
    public enum JSONType
    {
        Text,
        Compile,
        Status,
        OpenProject
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
