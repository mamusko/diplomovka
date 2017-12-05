using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplomovka
{
    class Procedure : Commands
    {
        Command procedureBody;
        string procedureName;

        public Procedure(string name, Command body)
        {
            this.procedureBody = body;
            this.procedureName = name;

        }
        public override void execute()
        {
            procedureBody.execute();
        }
    }
}
