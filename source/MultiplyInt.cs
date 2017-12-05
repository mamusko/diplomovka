using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplomovka
{
    class MultiplyInt : Commands
    {
        string variableName;
        int amount;
        Dictionary<string, int> variables;
        public MultiplyInt(string variableName, int value, Dictionary<string, int> variables)
        {
            this.variableName = variableName;
            this.amount = value;
            this.variables = variables;
        }
        public override void execute()
        {
            if (variables.ContainsKey(variableName))
            {
                int value = variables[variableName];
                variables[variableName] = value * amount;
                System.Diagnostics.Debug.WriteLine("");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("premenna " + variableName + " nieje zadefinovana");
            }

        }
    }
}
