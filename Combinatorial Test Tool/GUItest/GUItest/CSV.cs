using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUItest
{
    class CSV
    {
        public void SaveTests(string filename, string[,] data, Variable[] variables)
        {
            string filePath = @"C:\Users\Simon\Documents\Visual Studio 2013\Projects\GUItest\GUItest5\GUItest\CSVFiles\" + filename + ".csv";
            var strTest = new StringBuilder();
            string testVariable = "";
            int numTests = data.GetLength(0); // number of tests
            int numVar = data.GetLength(1); // number of variables

            for (int i = 0; i < numTests; i++)
            {
                strTest.Clear();
                for (int j = 0; j < numVar; j++)
                {
                    if (j < (numVar - 1))
                        testVariable = string.Format("{0},", data[i, j]);
                    else
                        testVariable = string.Format("{0}{1}", data[i, j], Environment.NewLine);
                    strTest.Append(testVariable);
                }
                System.IO.File.AppendAllText(filePath, strTest.ToString()); // write testcase to file
            }
            System.Windows.Forms.MessageBox.Show("File saved");
        }
    }
}
