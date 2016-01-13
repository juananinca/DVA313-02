using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUItest
{
    class CSV
    {
        private string extension = ".csv";
        private string GetFilePath(string filename, string fullPath) // add a number to the filename if another file with the same name already exist
        {
            int count = 1;
            string newFullPath = fullPath + filename + extension;
            while(System.IO.File.Exists(newFullPath))
            {
                string tempFilename = string.Format("{0}({1})", filename, count++);
                newFullPath = System.IO.Path.Combine(fullPath, tempFilename + extension);
            }
            return newFullPath;
        }

        public void SaveTests(string filename, List<List<string>> data /*all the testcases*/, Variable[] variables/*the datatypes from the xml file*/)
        {
            string dirPath = "..\\..\\..\\CSVFiles\\"; // directory path
            string fileFullPath;
            var strTest = new StringBuilder();
            string testVariable = "";
            int numTests = data.Count; // number of tests
            int numVar = variables.Length; // number of variables

            fileFullPath = GetFilePath(filename, dirPath); // full file path

            for (int i = 0; i < numVar; i++) // save names of variables to file
            {
                if (i < (numVar - 1))
                    strTest.Append(variables[i].name + ";");
                else
                    strTest.Append(variables[i].name + Environment.NewLine);
            }

            System.IO.File.AppendAllText(fileFullPath, strTest.ToString());

            for (int i = 0; i < numTests; i++)
            {
                strTest.Clear();
                for (int j = 0; j < numVar; j++)
                {
                    if (j < (numVar - 1))
                        testVariable = string.Format("{0};", data[i][j]);
                    else
                        testVariable = string.Format("{0}{1}", data[i][j], Environment.NewLine);
                    strTest.Append(testVariable);
                }
                System.IO.File.AppendAllText(fileFullPath, strTest.ToString()); // write testcase to file
            }
            System.Windows.Forms.MessageBox.Show("File saved");
        }
    }
}
