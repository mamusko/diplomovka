using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplomovka
{
    class ConditionInt : Commands
    {
        int argument1;
        int argument2;
        string conditionOperator;
        private Command isTrue;
        private Command isFalse;

        
        public ConditionInt(Expression argument1, Expression argument2, string conditionOperator, Command isTrue, Command isFalse)
        {
            this.argument1 = argument1.evaluate();
            this.argument2 = argument2.evaluate();
            this.conditionOperator = conditionOperator;
            this.isTrue = isTrue;
            this.isFalse = isFalse;
        }

        public override void execute()
        {
            if (conditionOperator == "=")
            {
                if (argument1 == argument2)
                {
                    isTrue.execute();
                }
                else
                {
                    isFalse.execute();
                }
            }
            else if (conditionOperator == "=<" || conditionOperator == "<=")
            {
                if (argument1 <= argument2)
                {
                    isTrue.execute();
                }
                else
                {
                    isFalse.execute();
                }
            }
            else if (conditionOperator == "=>" || conditionOperator == ">=")
            {
                if (argument1 >= argument2)
                {
                    isTrue.execute();
                }
                else
                {
                    isFalse.execute();
                }
            }
            else if (conditionOperator == "<>" || conditionOperator == "><")
            {
                if (argument1 != argument2)
                {
                    isTrue.execute();
                }
                else
                {
                    isFalse.execute();
                }
            }
            else if (conditionOperator == "<")
            {
                if (argument1 < argument2)
                {
                    isTrue.execute();
                }
                else
                {
                    isFalse.execute();
                }
            }
            else if (conditionOperator == ">")
            {
                if (argument1 > argument2)
                {
                    isTrue.execute();
                }
                else
                {
                    isFalse.execute();
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("neznamy operator " + conditionOperator);
            }
        }
    }
}
