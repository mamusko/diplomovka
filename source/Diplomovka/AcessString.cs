using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplomovka
{
    class AcessString: ExpressionStr
    {
        string variableName;
        Dictionary<string, string> variables;

        public AcessString(string name, Dictionary<string, string> variables)
        {
            this.variableName = name;
            this.variables = variables;
        }

        public override string evaluate()
        {
            return variables[variableName];
        }

        public override void trim()
        {
            variables[variableName] = variables[variableName].Trim();
        }
    }
}

