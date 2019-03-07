namespace Client.JSON
{
    public enum JsonType
    {
        Text,
        Compile,
        Status,
        OpenProject
    }

    class Json
    {
        public JsonType Type { set; get; }
        public string Data { set; get; }

        public Json(JsonType type, string data)
        {
            Type = type;
            Data = data;
        }
    }
}
