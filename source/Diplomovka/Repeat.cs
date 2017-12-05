using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplomovka
{
    class Repeat : Commands
    {
        Expression repeatCount;
        Command body;

        public Repeat(Expression count,Command body)
        {
            repeatCount = count;
            this.body = body;
        }
        public override void execute()
        {
            int count = repeatCount.evaluate();
            while (count > 0)
            {
                body.execute();
                count--;
            }
        }
    }
}
