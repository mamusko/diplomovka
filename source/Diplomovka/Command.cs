using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplomovka
{
    class Command : Commands
    {
        List<Commands> items;

        public Command(params Commands[] list)
        {
            items = new List<Commands>();
            for(int i = 0; i < list.Length; i++)
            {
                items.Add(list[i]);
            }
        }

        public void add(Commands item)
        {
            items.Add(item);
        }

        public override void execute()
        {
            foreach (Commands command in items)
            {
                command.execute();
            }
        }
    }
}
