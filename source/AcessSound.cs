using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplomovka
{
    class AcessSound
    {
        string variableName;
        Dictionary<string, Sound> variables;

        public AcessSound(string variableName, Dictionary<string, Sound> variables)
        {
            this.variableName = variableName;
            this.variables = variables;
        }

        public Sound evaluate()
        {
            return variables[variableName];
        }
    }
}
