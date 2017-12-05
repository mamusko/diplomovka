using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplomovka
{
    public abstract class BinaryOperation : Expression
    {
        protected Expression left;
        protected Expression right;

        public BinaryOperation(Expression l,Expression r)
        {
            left = l;
            right = r;
        }
    }
}
