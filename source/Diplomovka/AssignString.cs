using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplomovka
{
    class AssignString : Command
    {
        Dictionary<string, string> variables;
        string variableName;
        ExpressionStr expr;


        public AssignString(string name, ExpressionStr expr, Dictionary<string, string> variables)
        {
            variableName = name.Trim();
            this.expr = expr;
            this.variables = variables;
            variables[variableName] = expr.evaluate().Trim();
        }

        public override void execute()
        {
            variables[variableName] = expr.evaluate().Trim();
        }
    }
}
