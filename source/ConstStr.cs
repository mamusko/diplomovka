using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplomovka
{
    class ConstStr : ExpressionStr
    {
        string constantValue;
        public ConstStr(string value)
        {
            constantValue = value;
        }
        public override string evaluate()
        {
            return constantValue;
        }

        public override void trim()
        {
            constantValue = constantValue.Trim();
        }
    }
}
