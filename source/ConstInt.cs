using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplomovka
{
    class ConstInt : Expression
    {
        int constantValue;

        public ConstInt(int value)
        {
            constantValue = value;
        }
        public override int evaluate()
        {
            return constantValue;
        }
    }
}
