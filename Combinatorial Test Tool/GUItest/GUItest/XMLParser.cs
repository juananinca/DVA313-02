using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace GUItest
{
    public class XMLParser
    {
        public struct varData
        {
            public string name;
            public string type;
        }

        public string xmlString;
        private bool end = false;

        public string getText(string message)
        {
            List<varData> dataList = new List<varData>();

            dataList = readXMLDoc(dataList);

            foreach (varData item in dataList)
            {
                message += "Name: " + item.name + "\n" + "Type: " + item.type + "\n\n";
            }
            return message;
        }

        public List<varData> readXMLDoc(List<varData> dataList)
        {
            varData temp;

            using (XmlReader reader = XmlReader.Create(new StringReader(xmlString)))
            {
                reader.ReadToFollowing("inputVars");
                while (reader.Read() && !end)
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element: // The node is an element.
                            if (reader.Name == "variable")
                            {
                                reader.MoveToFirstAttribute();
                                temp.name = reader.Value;
                                reader.ReadToFollowing("type");
                                reader.Read();
                                reader.Read();
                                temp.type = reader.Name;
                                dataList.Add(temp);
                            }
                            break;
                        case XmlNodeType.EndElement:
                            if (reader.Name == "inputVars")
                            {
                                end = true;
                            }
                            break;
                    }
                }
            }
            return dataList;
        }
    }
}
