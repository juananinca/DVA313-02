using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUItest
{
    public class FormState
    {
        public string RangeStart;
        public string RangeEnd;
        public int DataTypeSelectedIndex;

        public string RootFolder;
        //public int FileSelectedIndex; // index of the seleced file in the listbox
        public string FullFilePath;

        public bool CheckBoxXML;

        public List<string> Ranges;
        public List<string> BaseValues;

        public int AlgorithmSelectedIndex;
        public Decimal NumTests;

        public List<List<string>> TestCases;
        public Variable[] VarList;


        public List<List<string>> InputGrid;
        public string Filename;
    }
}
