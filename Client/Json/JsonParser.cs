using System.Windows.Forms;

namespace Client.JSON
{
    static class JsonParser
    {
        private const string ONLINE_STATUS = "Online";
        private const string OFFILE_STATUS = "Offline";

        private static TextBox codeTextBox;
        private static TextBox resultTextBox;
        private static Label statusLabel;

        public static void SetProperties(TextBox code, TextBox result, Label status)
        {
            codeTextBox = code;
            resultTextBox = result;
            statusLabel = status;
        }

        public static void Parse(Json json)
        {
            switch (json.Type)
            {
                case JsonType.Text:
                    WriteToCodeTextBox(json.Data);
                    break;

                case JsonType.Compile:
                    WriteToResultTextBox(json.Data);
                    break;

                case JsonType.Status:
                    if (json.Data == ONLINE_STATUS)
                    {
                        WriteToStatusLabel(ONLINE_STATUS);
                    }
                    else if (json.Data == OFFILE_STATUS)
                    {
                        Client.CloseConnection();
                        WriteToStatusLabel(OFFILE_STATUS);
                    }
                    break;

                case JsonType.OpenProject:
                    Explorer.FillTreeView(json.Data);
                    break;
            }
        }

        private static void WriteToCodeTextBox(string text)
        {
            codeTextBox.Invoke((MethodInvoker)delegate
            {
                codeTextBox.Text = text;
            });
        }

        private static void WriteToResultTextBox(string text)
        {
            resultTextBox.Invoke((MethodInvoker)delegate
            {
                resultTextBox.Text = text;
            });
        }

        private static void WriteToStatusLabel(string text)
        {
            statusLabel.Invoke((MethodInvoker)delegate
            {
                statusLabel.Text = text;
            });
        }

        
    }
}
