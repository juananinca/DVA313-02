using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Text.RegularExpressions;

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

        /*public string getText(string message)
        {
            List<varData> dataList = new List<varData>();

            dataList = readXMLDoc(dataList);

            foreach (varData item in dataList)
            {
                message += "Name: " + item.name + "\n" + "Type: " + item.type + "\n\n";
            }
            return message;
        }*/

        public void SetXMLPath(string s)
        {
            this.xmlString = s;
        }

        public List<varData> readXMLDoc(List<varData> dataList, XmlDocument xmlDoc)
        {
            varData temp;
            XmlNodeList nodeList;
            xmlDoc = RemoveNS(xmlDoc);

            XmlElement root = xmlDoc.DocumentElement;
            nodeList = root.SelectNodes("/project/types/pous/pou/interface/inputVars/variable");
            foreach (XmlNode isbn in nodeList)
            {
                temp.name = isbn.Attributes["name"].Value;
                temp.type = isbn.FirstChild.FirstChild.Name;
                dataList.Add(temp);
            }
            return dataList;
        }

        private XmlDocument RemoveNS(XmlDocument doc)
        {
            var xml = doc.OuterXml;
            var newxml = Regex.Replace(xml, @"xmlns[:xsi|:xsd]*="".*?""", "");
            var newdoc = new XmlDocument();
            newdoc.LoadXml(newxml);
            return newdoc;
        }
    }
}
