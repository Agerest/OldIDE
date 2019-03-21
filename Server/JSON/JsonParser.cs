using Newtonsoft.Json;
using System;
using System.Collections.Generic;
namespace Server.JSON
{
    static class JsonParser
    {
        private const string ONLINE_STATUS = "Online";
        private const string OFFILE_STATUS = "Offline";

        public static void JsonParse(string jsonSerialize, Client client)
        {
            Json json = JsonConvert.DeserializeObject<Json>(jsonSerialize);

            try
            {
                switch (json.Type)
                {
                    case JSONType.text:
                        Server.CurrentText = json.Data;
                        Server.SendMessageAllUser(jsonSerialize, client);
                        break;
                    case JSONType.compile:
                        Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json.Data);
                        string message = Compiler.Compile(dictionary["program"], dictionary["name"]);
                        json = new Json(JSONType.compile, message);
                        string j = JsonConvert.SerializeObject(json);
                        client.SendMessage(j);
                        break;
                    case JSONType.status:
                        if (json.Data == OFFILE_STATUS) client.CloseConnection();
                        break;
                    case JSONType.openProject:
                        client.OpenProject(long.Parse(json.Data)); //Json.Data == project ID
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}