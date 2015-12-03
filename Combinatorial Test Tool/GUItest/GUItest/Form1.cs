using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Text.RegularExpressions;

namespace GUItest
{
    public partial class Form1 : Form
    {
        XMLParser Parser = new XMLParser();
        private string cellOldValue;
        private List<string> xmlFiles = new List<string>();
        private String sdira = @"C:\\Users\\Simon\\Documents\\Visual Studio 2013\\Projects\\GUItest\\GUItest\\XMLFiles";
        //private String sdira = @"C:\Users\Simon\Documents\Visual Studio 2013\Projects\GUItest\GUItest5\GUItest\XMLFiles";
        //private String sdira = @"C:\\Users\\Sara\\Downloads\\GUItest-3\\GUItest\\XMLFiles";

        //private String sdira = @"C:\Users\Sara\Dropbox\DVA313 Programvaruteknik 2\GUItest 20151130\GUItest\XMLFiles";
         
        private List<XMLParser.varData> dataList = new List<XMLParser.varData>();

        private Variable[] varList;
        private VariableBC[] varListBC;
        private string[,] tableTests;
        private string filename;
        public Form1()
        {
            InitializeComponent();
        }

        private void OnLoad(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            string[] arrays;
            arrays = Directory.GetFiles(sdira, "*", SearchOption.AllDirectories).Select(x => Path.GetFileName(x)).ToArray();

            /*foreach (string d in Directory.GetDirectories(sdira))
                foreach (string f in Directory.GetFiles(d, "*.*"))
                    xmlFiles.Add(f);*/

            //listBox1.DataSource = xmlFiles;
            listBox1.Items.AddRange(arrays);

            //base.OnLoad(e);
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();
            string filePath;

            if (result == DialogResult.OK)
            {
                filePath = openFileDialog1.FileName;
                Parser.xmlString = File.ReadAllText(filePath);
                dataList = Parser.readXMLDoc(dataList);

                string name = openFileDialog1.SafeFileName;
                filename = name.Substring(0, name.Length - 4);

                foreach (XMLParser.varData item in dataList)
                {
                    switch(comboBox1.SelectedIndex)
                    {
                        case 0: //base choice
                            dataGridView1.Rows.Add(item.name, item.type, "0", "0");
                            break;
                        case 1: //random choice
                            dataGridView1.Rows.Add(item.name, item.type, "0");
                            break;
                        default:
                            dataGridView1.Rows.Add(item.name, item.type, "0");
                            break;
                    }
                    //dataGridView1.Rows.Add(item.name, item.type, "0", "0");
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows.Count != 0 && (e.ColumnIndex == 2 || e.ColumnIndex == 3))
            {
                int i = 0;
                bool isNumeric;
                string cellContent = dataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString();
                //string regexText = null;
                string pattern = "[^;\n]*";
                MatchCollection matches = Regex.Matches(cellContent, pattern);
                /*foreach (Match match in matches)
                {
                    richTextBox1.AppendText(match.Value.ToString());
                    //regexText += match.Value;
                    //richTextBox1.Text = regexText;
                    string temp = match.Value.ToString();
                    isNumeric = int.TryParse(temp, out i);
                   if (!isNumeric)
                    {
                        MessageBox.Show("The input need to be numeric", "Wrong Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        dataGridView1[e.ColumnIndex, e.RowIndex].Value = cellOldValue;
                        break;
                    }
                }*/

                //dataGridView1[e.ColumnIndex, e.RowIndex].Value = "hej";
                isNumeric = int.TryParse(cellContent, out i);

                if (!isNumeric)
                {
                    //MessageBox.Show("The input need to be numeric", "Wrong Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //dataGridView1[e.ColumnIndex, e.RowIndex].Value = cellOldValue;
                }
            }
        }

        private void dataGridView1_CellLeave(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            cellOldValue = dataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString();
        }

        private void GenerateButton_Click(object sender, EventArgs e)
        {
            TestGenerator tg = new TestGenerator();
            int numTests = 0;
            int numInput = dataGridView1.Rows.Count;
            string rangeStr = "";
            string[] range = new string[2];

            varList = new Variable[numInput];
            for (int row = 0; row < dataGridView1.Rows.Count; row++) // read information from GUI
            {
                varList[row].name = dataGridView1.Rows[row].Cells[0].Value.ToString(); // name is in first column
                varList[row].datatype = dataGridView1.Rows[row].Cells[1].Value.ToString(); // datatype in second
                rangeStr = dataGridView1.Rows[row].Cells[2].Value.ToString(); // interval
                range = rangeStr.Split('-');
                varList[row].interval_a = range[0]; // start of interval 
                varList[row].interval_b = range[1]; // end of interval 
            }

            if(comboBox1.SelectedIndex == 0) // Base choice
            {
                varListBC = new VariableBC[numInput];

                for (int row = 0; row < dataGridView1.Rows.Count; row++)
                {
                    varListBC[row].variable = varList[row]; // add Variable to VariableBC list
                    varListBC[row].baseChoice = dataGridView1.Rows[row].Cells[3].Value.ToString(); // add base value to VariableBC list
                }

                tableTests = tg.GetBaseChoiceTests(varListBC); // generate base choice test cases
                numTests = tg.GetNumTestCases(); // get number of tests, which i calculated in GetBaseChoiceTests

            }
            else if (comboBox1.SelectedIndex == 1) // Random
            {
                numTests = Decimal.ToInt32(numericUpDown1.Value); // get number of tests from GUI
                tableTests = tg.GetRandomTests(numTests, varList); // generate random test cases
            }

            dataGridView2.Columns.Clear(); // empty dataGridView
            dataGridView2.Rows.Clear();

            for (int i = 0; i < numInput; i++) // add a column for each input
            {
                dataGridView2.Columns.Add("Column", varList[i].name + "(" + varList[i].datatype + ")");
            }

            for (int row = 0; row < numTests; row++) // set test to each cell in the dataGridView (one test case per row)
            {
                dataGridView2.Rows.Add();
                for (int col = 0; col < numInput; col++)
                {
                    dataGridView2.Rows[row].Cells[col].Value = tableTests[row, col];
                }
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if(dataGridView1.Rows[i].Cells[1].Value.ToString() == comboBox2.SelectedItem.ToString() && FromTextBox.Text != "" && ToTextBox.Text != "")
                {
                    switch (dataGridView1.Rows[i].Cells[2].Value.ToString())
                    {
                        case "0":
                            dataGridView1.Rows[i].Cells[2].Value = FromTextBox.Text + '-' + ToTextBox.Text;
                            break;

                        default:
                            dataGridView1.Rows[i].Cells[2].Value += ';' + FromTextBox.Text + '-' + ToTextBox.Text;
                            break;
                    }
                }
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            //dataGridView1.Rows.Add("1", "2", "3", "4");
            for(int i = 0; i < dataGridView1.Rows.Count; i++) //save/overwrite range to all inputs of comboBox2 selected item
            {
                if (dataGridView1.Rows[i].Cells[1].Value.ToString() == comboBox2.SelectedItem.ToString())
                    dataGridView1.Rows[i].Cells[2].Value = FromTextBox.Text + '-' + ToTextBox.Text;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(comboBox1.SelectedIndex)
            {
                case 0: //base choice
                    dataGridView1.Columns.Add(Column4);
                    numericUpDown1.Visible = false;
                    label7.Visible = false;
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                        dataGridView1.Rows[i].Cells[3].Value = "0";
                        break;
                case 1: //random choice
                    dataGridView1.Columns.Remove(Column4);
                    numericUpDown1.Visible = true;
                    label7.Visible = true;
                    break;
                default:
                    numericUpDown1.Visible = false;
                    label7.Visible = false;
                    break;
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string filePath = sdira + "\\" + listBox1.SelectedItem;
            Parser.xmlString = File.ReadAllText(filePath);
            dataList = Parser.readXMLDoc(dataList);

            dataGridView1.Rows.Clear();
            foreach (XMLParser.varData item in dataList)
            {
                switch (comboBox1.SelectedIndex)
                {
                    case 0: //base choice
                        dataGridView1.Rows.Add(item.name, item.type, "0", "0");
                        break;
                    case 1: //random choice
                        dataGridView1.Rows.Add(item.name, item.type, "0");
                        break;
                    default:
                        dataGridView1.Rows.Add(item.name, item.type, "0");
                        break;
                }
            }
            string name = listBox1.SelectedItem.ToString();
            filename = name.Substring(0, name.Length - 4); // removes the last 4 characters (.xml) from the filename 
        }

        private void listBox1_Enter(object sender, EventArgs e)
        {
            /*string filePath = sdira + listBox1.SelectedItem;
            Parser.xmlString = File.ReadAllText(filePath);
            dataList = Parser.readXMLDoc(dataList);

            foreach (XMLParser.varData item in dataList)
            {
                switch (comboBox1.SelectedIndex)
                {
                    case 0: //base choice
                        dataGridView1.Rows.Add(item.name, item.type, "0", "0");
                        break;
                    case 1: //random choice
                        dataGridView1.Rows.Add(item.name, item.type, "0");
                        break;
                    default:
                        dataGridView1.Rows.Add(item.name, item.type, "0");
                        break;
                }
            }*/
        }

        private void button3_Click(object sender, EventArgs e)
        {
            CSV csv = new CSV();
            csv.SaveTests(filename, tableTests, varList);
        }
    }
}
