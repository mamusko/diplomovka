using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplomovka
{
    class AssignSound : Command
    {
        Sound sound;
        string variableName;
        Dictionary<string, Sound> variables;

        public AssignSound(string name, Sound sound,Dictionary<string, Sound> variables)
        {
            variableName = name.Trim();
            this.sound = sound;
            this.variables = variables;
            variables[variableName] = sound;
        }

        public override void execute()
        {
            variables[variableName] = sound;
        }
    }
}
