using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplomovka
{
    class WhileInt : Commands
    {
        Expression test1;
        Expression test2;
        Command body;
        string conditionOperator;

        public WhileInt(Expression test1, Expression test2, string opr, Command body)
        {
            this.test1 = test1;
            this.test2 = test2;
            this.body = body;
            conditionOperator = opr;
        }
        public override void execute()
        {
            if (conditionOperator == "=")
            {
                while (test1.evaluate() == test2.evaluate())
                {
                    body.execute();
                }
            }
            else if (conditionOperator == "=<" || conditionOperator == "<=")
            {
                while (test1.evaluate() <= test2.evaluate())
                {
                    body.execute();
                }
            }
            else if (conditionOperator == "=>" || conditionOperator == ">=")
            {
                while (test1.evaluate() >= test2.evaluate())
                {
                    body.execute();
                }
            }
            else if (conditionOperator == "<>" || conditionOperator == "><")
            {
                while (test1.evaluate() != test2.evaluate())
                {
                    body.execute();
                }
            }
            else if (conditionOperator == "<")
            {
                while (test1.evaluate() < test2.evaluate())
                {
                    body.execute();
                }
            }
            else if (conditionOperator == ">")
            {
                while (test1.evaluate() > test2.evaluate())
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
