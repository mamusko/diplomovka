using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplomovka
{
    class Question : Commands
    {
        Command ifTrue;
        Command ifFalse;
        ExpressionStr questionText;
        public Question(ExpressionStr text, Command ifTrue, Command ifFalse)
        {
            questionText = text;
            this.ifTrue = ifTrue;
            this.ifFalse = ifFalse;
        }
        public override void execute()
        {
            // todo dialogove okno
            ifFalse.execute();
        }
    }
}
