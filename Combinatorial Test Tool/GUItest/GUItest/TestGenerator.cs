using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUItest
{
    public struct Variable
    {
        public string name;
        public string datatype;
        public string interval_a;
        public string interval_b;
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
            int n = varList.Length; // number of inputs
            int[,] range = new int[n, 2];
            int[] baseValues = new int[n]; 
            int[,] arr;
            string[,] table;
            int k = 0, m = 0, q = 0;

            numTestCases = 0;
            range = getIntervals(varList); // get start and end of all intervals
            baseValues = getBaseValues(varList); // get all base values

            for (int i = 0; i < range.GetLength(0); i++) 
            {
                numTestCases = numTestCases + (range[i, 1] - range[i, 0]);
            }
            numTestCases = numTestCases + 1; // number of test cases

            arr = new int[numTestCases, n];

            for (int i = 0; i < n; i++)
            {
                arr[numTestCases - 1, i] = baseValues[i];
            }

            // generate the base choice test cases 
            for (int i = 0; i < n; i++) 
            {
                for (int a = range[i, 0]; a <= range[i, 1]; a++)
                {
                    if (baseValues[i] != a)
                    {
                        arr[k, i] = a;
                        k++;
                    }

                }
                for (int j = 0; j < n; j++)
                {
                    if (i != j)
                    {
                        for (m = q; m < k; m++)
                        {
                            arr[m, j] = baseValues[j];
                        }
                    }
                }
                q = m;
            }

            table = new string[numTestCases, n];

            for (int i = 0; i < numTestCases; i++) // save the generated test cases in a 2d-array of strings
            {
                for (int j = 0; j < n; j++)
                {
                    table[i, j] = arr[i, j].ToString();
                }
            }

            return table;
        }

        public int GetNumTestCases()
        {
            return numTestCases;
        }

        private int[] getBaseValues(VariableBC[] varList)
        {
            int numInputs = varList.Length;
            int[] baseValues = new int[numInputs];
            for (int i = 0; i < numInputs; i++)
            {
                baseValues[i] = Int32.Parse(varList[i].baseChoice);
            }
            return baseValues;
        }

        private int[,] getIntervals(VariableBC[] varList)
        {
            int numInputs = varList.Length;
            int[,] intervals = new int[numInputs, 2];
            for (int i = 0; i < numInputs; i++)
            {
                intervals[i, 0] = Int32.Parse(varList[i].variable.interval_a);
                intervals[i, 1] = Int32.Parse(varList[i].variable.interval_b);
            }
            return intervals;
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
            switch (variable.datatype)
            {
                case "INT":
                    return getRandom_int(variable);
                case "BOOL":
                    return getRandom_bool(variable);
                case "LREAL":
                    return getRandom_double(variable);
                case "REAL":
                    return getRandom_float(variable);
                default:
                    return "-";
            }
        }
        private string getRandom_int(Variable v)
        {
            int min = Int32.Parse(v.interval_a);
            int max = Int32.Parse(v.interval_b);
            int r = random.Next(min, (max + 1));
            return r.ToString();
        }
        private string getRandom_bool(Variable v)
        {
            int min = Int32.Parse(v.interval_a);
            int max = Int32.Parse(v.interval_b);
            int r = random.Next(min, (max + 1));
            return r.ToString();
        }

        private string getRandom_double(Variable v)
        {
            double min = Double.Parse(v.interval_a);
            double max = Double.Parse(v.interval_b);
            double r = min + random.NextDouble() * (max - min);
            return r.ToString().Replace(",", "."); ;
        }

        private string getRandom_float(Variable v)
        {
            float min = float.Parse(v.interval_a);
            float max = float.Parse(v.interval_b);
            float r = min + ((float)(random.NextDouble())) * (max - min);
            return r.ToString().Replace(",", ".");
        }
    }
}
