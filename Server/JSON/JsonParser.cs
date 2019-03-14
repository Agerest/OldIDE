using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.JSON
{
    static class JsonParser
    {
        private const string ONLINE_STATUS = "Online";
        private const string OFFILE_STATUS = "Offline";
        public static void JsonParse(string jsonSerialize, User user)
        {
            Json json = JsonConvert.DeserializeObject<Json>(jsonSerialize);

            try
            {
                switch (json.Type)
                {
                    case JSONType.text:
                        Server.CurrentText = json.Data;
                        Server.SendMessageAllUser(jsonSerialize, user);
                        break;
                    case JSONType.compile:
                        Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json.Data);
                        string message = Compiler.Compile(dictionary["program"], dictionary["name"]);
                        json = new Json(JSONType.compile, message);
                        string j = JsonConvert.SerializeObject(json);
                        user.SendMessage(j);
                        break;
                    case JSONType.status:
                        if (json.Data == OFFILE_STATUS) user.CloseConnection();
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