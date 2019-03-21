using System.Xml;

namespace Server.JSON
{
    class JsonProject
    {
        private long ID { get; set; }
        private JsonClass[] Classes { get; set; }
        private XmlDocument Xml { get; set; }

        public JsonProject(long id, JsonClass[] classes, XmlDocument xml)
        {
            ID = id;
            Classes = classes;
            Xml = xml;
        }

    }
}
