using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUItest
{
    public struct Interval
    {
        public string interval_a;
        public string interval_b;
    }
    public struct Variable
    {
        public string name;
        public string datatype;
        public Interval[] intervals;
    }

    public struct VariableBC
    {
        public Variable variable;
        public string baseChoice;
    }

    class TestGenerator
    {
        private Random random = new Random();
        private int numTestCases;

        public string[,] GetBaseChoiceTests(VariableBC[] varList)
        {
            int numInput = varList.Length; // number of inputs
            string[] baseValues = new string[numInput];
            string[,] testCases;
            int k = 0, m = 0, q = 0;
            List<string>[] intervals = new List<string>[numInput]; // an array of lists, where each list contains all the values within the interval(s) for an input
            List<string> allValues;

            baseValues = getBaseValues(varList); // get all base values

            numTestCases = 0;
            for (int i = 0; i < numInput; i++)
            {
                intervals[i] = getAllFromIntervals(varList[i]); // all the values within the intervals, e.g. 2-5;7-9;17-19 -> (2,3,4,5,7,8,9,17,18,19)
                numTestCases = numTestCases + intervals[i].Count;
                //printListStr(intervals[i]); // test function just to print all the values within the intervals of a Variable
            }
            numTestCases = numTestCases - numInput + 1; // calculate the number of test cases

            testCases = new string[numTestCases, numInput]; // size of array which will contain the test cases, where each row is a test

            for (int i = 0; i < numInput; i++) // add the combination of all base values as the last test case
            {
                testCases[numTestCases - 1, i] = baseValues[i];
            }

            // generate the base choice test cases 
            for (int i = 0; i < numInput; i++)
            {
                allValues = getUniqueList(intervals[i]);  // list of all the values within the interval(s) for an input (doesn't contain any duplicates)
                //printListStr(allValues);
                //System.Windows.Forms.MessageBox.Show("All values count: " + allValues.Count);
                for (int a = 0; a < allValues.Count; a++)
                {
                    if (baseValues[i] != allValues.ElementAt(a))
                    {
                        testCases[k, i] = allValues.ElementAt(a);
                        k++;
                    }
                }
                for (int j = 0; j < numInput; j++)
                {
                    if (i != j)
                    {
                        for (m = q; m < k; m++)
                        {
                            testCases[m, j] = baseValues[j];
                        }
                    }
                }
                q = m;
            }
            return testCases;
        }

        private List<string> getUniqueList(List<string> list)
        {
            HashSet<string> hs = new HashSet<string>();
            for (int i = 0; i < list.Count; i++)
            {
                hs.Add(list.ElementAt(i));
            }
            return hs.ToList();
        }

        public int GetNumTestCases()
        {
            return numTestCases;
        }

        private List<string> getAllFromIntervals(VariableBC v) // get all the values within the intervals from a variable to an array
        {
            switch (v.variable.datatype)
            {
                case "INT":
                    return getAllValues_Int(v.variable.intervals);
                case "BOOL":
                    return getAllValues_Int(v.variable.intervals);
                case "REAL":
                    return getAllValues_Float(v.variable.intervals);
                default:
                    List<string> s = new List<string>();
                    s.Add("Error");
                    return s;
            }
        }

        private List<string> getAllValues_Int(Interval[] intervals) // returns a list of all the values within the intervals
        {
            int a = 0, b = 0;
            List<string> list = new List<string>();
            for (int i = 0; i < intervals.Length; i++)
            {
                a = Int32.Parse(intervals[i].interval_a);
                b = Int32.Parse(intervals[i].interval_b);
                while (a <= b)
                {
                    list.Add(a.ToString());
                    a = a + 1;
                }
            }
            return list;
        }

        private List<string> getAllValues_Float(Interval[] intervals) // returns a list of all the values within the intervals
        {
            float a = 0.0f, b = 0.0f;
            List<string> list = new List<string>();
            for (int i = 0; i < intervals.Length; i++)
            {
                a = float.Parse(intervals[i].interval_a);
                b = float.Parse(intervals[i].interval_b);
                while (a <= b)
                {
                    a = float.Parse(Math.Round((Decimal)a, 1) + "");
                    list.Add(a.ToString("0.0"));
                    a = a + 0.1f;
                }
            }
            return list;
        }

        private void printListStr(List<string> list)
        {
            string elements = "";
            for (int i = 0; i < list.Count; i++)
            {
                elements = elements + list.ElementAt(i) + " ";
            }
            System.Windows.Forms.MessageBox.Show("All elements within ranges: " + elements);
        }

        private string[] getBaseValues(VariableBC[] varList)
        {
            int numInputs = varList.Length;
            string[] baseValues = new string[numInputs];
            for (int i = 0; i < numInputs; i++)
            {
                baseValues[i] = varList[i].baseChoice;
            }
            return baseValues;
        }

        public string[,] GetRandomTests(int numTests, Variable[] varList)
        {
            string[,] table = new string[numTests, varList.Length];
            for (int i = 0; i < numTests; i++) // loop for each test
            {
                for (int j = 0; j < varList.Length; j++)
                {
                    table[i, j] = GetRandom(varList[j]); // generate a random value for all the inputs in the list
                }
            }
            return table;
        }

        private string GetRandom(Variable variable)
        {
            int iIndex = getRandomIntervalIndex(variable.intervals.Length);
            Interval interval = variable.intervals[iIndex];
            switch (variable.datatype)
            {
                case "INT":
                case "BOOL":
                    return getRandom_int(interval);
                    //return getRandom_bool(interval);
                //case "LREAL":
                //    return getRandom_double(interval);
                case "REAL":
                    return getRandom_float(interval);
                default:
                    return "-";
            }
        }

        private int getRandomIntervalIndex(int numIntervals)
        {
            return random.Next(0, numIntervals);
        }

        private string getRandom_int(Interval interval)
        {
            int min = Int32.Parse(interval.interval_a);
            int max = Int32.Parse(interval.interval_b);
            int r = random.Next(min, (max + 1));
            return r.ToString();
        }
        private string getRandom_bool(Interval interval)
        {
            int min = Int32.Parse(interval.interval_a);
            int max = Int32.Parse(interval.interval_b);
            int r = random.Next(min, (max + 1));
            return r.ToString();
        }

        private string getRandom_double(Interval interval)
        {
            double min = Double.Parse(interval.interval_a);
            double max = Double.Parse(interval.interval_b);
            double r = min + random.NextDouble() * (max - min);
            return r.ToString("0.0"); ;
        }

        private string getRandom_float(Interval interval)
        {
            float min = float.Parse(interval.interval_a);
            float max = float.Parse(interval.interval_b);
            float r = min + ((float)(random.NextDouble())) * (max - min);
            return r.ToString("0.0");
        }
    }
}
