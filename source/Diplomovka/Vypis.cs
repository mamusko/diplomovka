using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Diplomovka
{

    class Vypis : Commands
    {
        string variableName;
        Dictionary<string, int> variablesInt;
        Dictionary<string, string> variablesString;
        Dictionary<string, Sound> variablesSound;
        RichTextBox tb;
        public Vypis(String text, Dictionary<string, int> variablesInt, Dictionary<string, string> variablesString, Dictionary<string, Sound> variablesSound, RichTextBox tb)
        {
            variableName = text;
            this.variablesInt = variablesInt;
            this.variablesString = variablesString;
            this.variablesSound = variablesSound;
            this.tb = tb;
        }

        public override void execute()
        {
            if (variablesInt.ContainsKey(variableName))
            {
                //System.Diagnostics.Debug.WriteLine(variableName + " = " + variablesInt[variableName]);
                tb.Text += variableName + " = " + variablesInt[variableName] + "\n";
            }
            else if (variablesString.ContainsKey(variableName))
            {
                System.Diagnostics.Debug.WriteLine(variableName + " = " + variablesString[variableName]);
                tb.Text += variableName + " = " + variablesString[variableName] + "\n";
            }
            else if (variablesSound.ContainsKey(variableName))
            {
                System.Diagnostics.Debug.WriteLine(variableName + " = " + variablesSound[variableName]);
                tb.Text += variableName + " = " + variablesSound[variableName].soundName + "\n";
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("nepoznam premennu " + variableName + "\n");
            }
        }
    }
}

