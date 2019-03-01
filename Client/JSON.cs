namespace Client
{
    public enum JSONType
    {
        text,
        compile,
        cmd
    }

    class JSON
    {
        public JSONType Type { get; }
        public string Data { get; }

        public JSON(JSONType type, string data)
        {
            Type = type;
            Data = data;
        }
    }
}
