using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplomovka
{
    class AccessInt : Expression
    {
        string variableName;
        Dictionary<string, int> variables;
        
        public AccessInt(string name, Dictionary<string,int> variables)
        {
            this.variableName = name;
            this.variables = variables;
        }

        public override int evaluate()
        {
            return variables[variableName];
        }
    }
}
