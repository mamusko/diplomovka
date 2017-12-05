using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Diplomovka
{
    class Zahraj : Command
    {
        Sound sound;
        RichTextBox tb;

        public Zahraj(Sound sound, RichTextBox tb)
        {
            this.sound = sound;
            this.tb = tb;
        }
        public override void execute()
        {
            System.Diagnostics.Debug.WriteLine("Zahraj " + sound.soundName);
            tb.Text += "Zahraj " + sound.soundName + "\n";

       }
    }
}
