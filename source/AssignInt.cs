using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplomovka
{
    class AssignInt : Command
    {
        string variableName;
        Expression expr;
        Dictionary<string, int> variables;

        public AssignInt(string name,Expression expr, Dictionary<string, int> variables)
        {
            variableName = name;
            this.expr = expr;
            this.variables = variables;
            variables[variableName] = expr.evaluate();
        }


        public override void execute()
        {
            variables[variableName] = expr.evaluate();
        }

    }
}
