using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplomovka
{
    class ConditionString : Commands
    {
        ExpressionStr test1;
        ExpressionStr test2;
        string conditionOperator;
        private Command isTrue;
        private Command isFalse;

        public ConditionString(ExpressionStr test1, ExpressionStr test2, string conditionOperator, Command isTrue, Command isFalse)
        {
            this.test1 = test1;
            test1.trim();
            this.test2 = test2;
            test2.trim();
            this.conditionOperator = conditionOperator;
            this.isTrue = isTrue;
            this.isFalse = isFalse;
        }
        public override void execute()
        {
            if (conditionOperator == "=")
            {
                if (test1.evaluate() == test2.evaluate())
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
                if(test1.evaluate().Trim() != test2.evaluate().Trim())
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
