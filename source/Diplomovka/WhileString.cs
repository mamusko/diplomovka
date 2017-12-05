using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplomovka
{
    class WhileString:Commands
    {
        ExpressionStr argument1;
        ExpressionStr argument2;
        string conditionOperator;
        private Command body;
        Dictionary<string, string> variablesString = new Dictionary<string, string>();

        public WhileString(ExpressionStr arg1, ExpressionStr arg2, string conditionOperator, Dictionary<string, string> variablesString, Command body)
        {
            argument1 = arg1;
            argument1.trim();
            argument2 = arg2;
            argument2.trim();
            this.conditionOperator = conditionOperator;
            this.body = body;
        }
        public override void execute()
        {
            if (conditionOperator == "=")
            {
                while (argument1.evaluate() == argument2.evaluate())
                {
                    body.execute();
                }
            }
            else if (conditionOperator == "<>" || conditionOperator == "><")
            {
               while (argument1.evaluate().Trim() != argument2.evaluate().Trim())
                {
                    body.execute();
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("neznamy operator " + conditionOperator);
            }
        }
    }
}