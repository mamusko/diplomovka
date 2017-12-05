using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Diplomovka
{
    class Povedz: SoundCommand
    {
        string textToSay;
        RichTextBox tb;
        public Povedz(string text, RichTextBox tb)
        {
            textToSay = text;
            this.tb = tb;
        }

        public override void execute()
        {
            System.Diagnostics.Debug.WriteLine("Povedz " + textToSay);
            tb.Text += "Povedz " + textToSay + "\n";
        }
    }
}
