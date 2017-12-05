using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplomovka
{
    class EnterInt : Commands
    {
        string query;
        Expression value;
        public EnterInt(string text)
        {
            this.query = text;
        }
        public override void execute()
        {
            
        }
    }
}
