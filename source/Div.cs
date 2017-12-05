using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplomovka
{
    class Div : BinaryOperation
    {
        public Div(Expression l, Expression r) : base(l, r)
        {
            left = l;
            right = r;
        }

        public override int evaluate()
        {
            return left.evaluate() / right.evaluate();
        }
    }
}
