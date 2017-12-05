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
    class Compiler
    {
        public const int NOTHING = 0;
        public const int NUMBER = 1;
        public const int WORD = 2;
        public const int SYMBOL = 3;
        public bool compilationSucess = true;
        string token;
        int kind;
        string programText;
        int position;
        int lineNumber = 1;
        int index;
        char look;
        RichTextBox outputTextbox;
        RichTextBox inputTextbox;
        Dictionary<string, Command> procedures = new Dictionary<string, Command>();
        Dictionary<string, int> variablesInt = new Dictionary<string, int>();
        Dictionary<string, string> variablesString = new Dictionary<string, string>();
        Dictionary<string, Sound> variablesSound = new Dictionary<string, Sound>();
        Dictionary<string, string> sounds = new Dictionary<string, string>();
        HashSet<string> variableNames = new HashSet<string>();
        HashSet<string> commands = new HashSet<string>();

        public Compiler(RichTextBox input, RichTextBox output)
        {
            loadSounds();
            loadCommands();
            this.inputTextbox = input;
            this.outputTextbox = output;
            programText = inputTextbox.Text.ToLower();
            index = 0;
            next();
            scan();
        }

        private void loadSounds()
        {
            string currentDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            string route = Path.Combine(currentDirectory, "Zvuky\\");
            string [] folderNames = Directory.GetDirectories(route);
            for (int i = 0; i < folderNames.Length; i++)
            {
                string [] soundNames = Directory.GetFiles(folderNames[i], "*.wav");
                for (int j = 0; j< soundNames.Length; j++)
                {
                    sounds.Add(trimSoundName(soundNames[j]), trimFolderName(folderNames[i]));
                }
            }
        }

        private string trimSoundName(string input)
        {
            int unicode = 92;
            char character = (char)unicode;
            int index = 0;
            string tmp;
            for (int i = input.Length - 1; i > 0; i--)
            {
                if (input[i] == character)
                {
                    index = i;
                    break;
                }
            }
            tmp = input.Remove(0, index + 1);
            tmp = tmp.Remove(tmp.Length - 4);
            return tmp;
        }

        private string trimFolderName(string input)
        {
            int unicode = 92;
            char character = (char)unicode;
            int index = 0;
            string tmp;
            for (int i = input.Length - 1; i > 0; i--)
            {
                if (input[i] == character)
                {
                    index = i;
                    break;
                }
            }
            tmp = input.Remove(0, index + 1);
            return tmp;
        }

        private void loadCommands()
        {
            commands.Add("zahraj");
            commands.Add("povedz");
            commands.Add("opakuj");
            commands.Add("otazka");
            commands.Add("ak");
            commands.Add("pripocitaj");
            commands.Add("odpocitaj");
            commands.Add("cislo");
            commands.Add("text");
            commands.Add("vypis");
        }

        public Command compile()
        {
            Command result = new Command();
            while (kind == WORD)
            {
                if (token == "zahraj")
                {
                    scan();
                    if (token == "(")
                    {
                        scan();
                    }
                    else
                    {
                        outputTextbox.Text += "Chyba na riadku " + lineNumber + ": po prikaze zahraj nasleduje znak ( \n";
                        compilationSucess = false;
                    }
                    if (sounds.ContainsKey(token))
                    {
                        Sound sound = new Sound(token, sounds[token]);
                        result.add(new Zahraj(sound, outputTextbox));
                        scan();
                    }
                    else
                    {
                        outputTextbox.Text += "Chyba na riadku " + lineNumber + ": nepoznam zvuk " + token + "\n";
                        compilationSucess = false;
                        scan();
                    }
                    if (token == ")")
                    {
                        scan();
                    }
                    else
                    {
                        outputTextbox.Text += "Chyba na riadku " + lineNumber + ": prikaz zahraj treba ukoncit znakom ) \n";
                        compilationSucess = false;
                    }
                }
                else if (token == "povedz")
                {
                    scan();
                    if (token == "(")
                    {
                        scan();
                    }
                    else
                    {
                        outputTextbox.Text += "Chyba na riadku " + lineNumber + ": po prikaze povedz nasleduje znak ( \n";
                        compilationSucess = false;
                    }
                    result.add(new Povedz(token, outputTextbox));
                    scan();
                    if (token == ")")
                    {
                        scan();
                    }
                    else
                    {
                        outputTextbox.Text += "Chyba na riadku " + lineNumber + ": prikaz povedz treba ukoncit znakom ) \n";
                        compilationSucess = false;
                    }
                }
                else if (token == "opakuj")
                {
                    scan();
                    Expression repeatCount = addSub();
                    if (repeatCount == null)
                    {
                        outputTextbox.Text += "Chyba na riadku " + lineNumber + ": zadajte pocet opakovani \n";
                        compilationSucess = false;
                    }
                    if (token == "krat")
                    {
                        scan();
                    }
                    else
                    {
                        outputTextbox.Text += "Chyba na riadku " + lineNumber + ": po pocte opakovani nasleduje slovo krat \n";
                        compilationSucess = false;
                    }
                    Command body = compile();
                    if (token == "koniec")
                    {
                        scan();
                        result.add(new Repeat(repeatCount, body));
                    }

                }
                else if (token == "otazka")
                {
                    scan();
                    if (token == "(")
                    {
                        scan();
                    }
                    else
                    {
                        outputTextbox.Text += "Chyba na riadku " + lineNumber + ": po prikaze otazka nasleduje znak ( \n";
                        compilationSucess = false;
                    }
                    ExpressionStr text = scanAllTextUntilBrace();
                    if (token == ")")
                    {
                        scan();
                    }
                    else
                    {
                        outputTextbox.Text += "Chyba na riadku " + lineNumber + ": prikaz otazka je treba ukoncit znakom ) \n";
                        compilationSucess = false;
                    }
                    if (token == "je")
                    {
                        scan();
                    }
                    else
                    {
                        outputTextbox.Text += "Chyba na riadku " + lineNumber + ": po prikaze otazka nasleduje slovo je \n";
                        compilationSucess = false;
                    }
                    if (token == "ano")
                    {
                        scan();
                    }
                    else
                    {
                        outputTextbox.Text += "Chyba na riadku " + lineNumber + ": po prikaze otazka je nasleduje slovo ano ( \n";
                        compilationSucess = false;
                    }

                    Command isTrue = compile();
                    if (token == "inak")
                    {
                        scan();
                        Command isFalse = compile();
                        if (token == "koniec")
                        {
                            result.add(new Question(text, isTrue, isFalse));
                            scan();
                        }
                        else
                        {
                            outputTextbox.Text += "Chyba na riadku " + lineNumber + ": prikaz otazka musi byt ukonceny slovom koniec \n";
                            compilationSucess = false;
                        }
                    }
                    else
                    {
                        outputTextbox.Text += "Chyba na riadku " + lineNumber + ": prikaz otazka musi obsahovat klucove slovo inak \n";
                        compilationSucess = false;
                    }
                }
                else if (token == "ak")
                {
                    scan();
                    if (token == "(")
                    {
                        scan();
                    }
                    else
                    {
                        outputTextbox.Text += "Chyba na riadku " + lineNumber + ": po prikaze ak nasleduje znak ( \n";
                        compilationSucess = false;
                    }
                    if (int.TryParse(token, out int cislo) || variablesInt.ContainsKey(token))
                    {
                        Commands temp = addConditionInt();
                        if (temp != null)
                        {
                            result.add(temp);
                        }
                    }
                    else
                    {
                        Commands temp = addConditionString();
                        if (temp != null)
                        {
                            result.add(temp);
                        }
                    }
                }
                else if (token == "kym")
                {
                    scan();
                    if (token == "(")
                    {
                        scan();
                    }
                    else
                    {
                        outputTextbox.Text += "Chyba na riadku " + lineNumber + ": po prikaze kym nasleduje znak ( \n";
                        compilationSucess = false;
                    }
                    if (int.TryParse(token, out int cislo) || variablesInt.ContainsKey(token))
                    {
                        Commands temp = addWhileInt();
                        if (temp != null)
                        {
                            result.add(temp);
                        }
                    }
                    else
                    {
                        Commands temp = addWhileString();
                        if (temp != null)
                        {
                            result.add(temp);
                        }
                    }
                }
                else if (token == "urob")
                {
                    scan();
                    if (!variableNames.Contains(token))
                    {
                        string procedureName = token;
                        scan();
                        Command body = compile();
                        if (token == "koniec")
                        {
                            if (!variableNames.Contains(procedureName))
                            {
                                variableNames.Add(procedureName);
                                procedures.Add(procedureName, body);
                                scan();
                            }
                            else
                            {
                                outputTextbox.Text += "Chyba na riadku " + lineNumber + ": premenna + " + procedureName + " uz je definovana \n";
                                compilationSucess = false;
                            }
                        }
                        else
                        {
                            outputTextbox.Text += "Chyba na riadku " + lineNumber + ": prikaz urob nebol ukonceny prikazom koniec\n";
                            compilationSucess = false;
                        }

                    }
                    else
                    {
                        outputTextbox.Text += "Chyba na riadku " + lineNumber + ": nazov " + token + " je obsadeny\n";
                        compilationSucess = false;
                    }
                }
                else if (token == "pripocitaj")
                {
                    scan();
                    if (token == "(")
                    {
                        scan();
                    }
                    else
                    {
                        outputTextbox.Text += "Chyba na riadku " + lineNumber + ": po prikaze ak nasleduje znak ( \n";
                        compilationSucess = false;
                    }
                    string variableName = token;
                    if (variablesInt.ContainsKey(variableName))
                    {
                        scan();
                        if (token != ",")
                        {
                            outputTextbox.Text += "Chyba na riadku " + lineNumber + ": funkcia pripocitaj vyzaduje 2 argumenty \n";
                            compilationSucess = false;
                        }
                        scan();
                        result.add(new IncrementInt(variableName, express().evaluate(), variablesInt));
                        if (token == ")")
                        {
                            scan();
                        }
                        else
                        {
                            outputTextbox.Text += "Chyba na riadku " + lineNumber + ": funkcia pripocitaj nieje ukoncena znakom )\n";
                            compilationSucess = false;
                        }
                    }
                    else
                    {
                        outputTextbox.Text += "Chyba na riadku " + lineNumber + ": nepoznam premennu " + variableName + "\n";
                        compilationSucess = false;
                    }
                }
                else if (token == "odpocitaj")
                {
                    scan();
                    if (token == "(")
                    {
                        scan();
                    }
                    else
                    {
                        outputTextbox.Text += "Chyba na riadku " + lineNumber + ": po prikaze odpocitaj nasleduje znak ( \n";
                        compilationSucess = false;
                    }
                    string variableName = token;
                    if (variablesInt.ContainsKey(variableName))
                    {
                        scan();
                        if (token != ",")
                        {
                            outputTextbox.Text += "Chyba na riadku " + lineNumber + ": funkcia odpocitaj vyzaduje 2 argumenty \n";
                            compilationSucess = false;
                        }
                        scan();
                        result.add(new DecrementInt(variableName, express().evaluate(), variablesInt));
                        if (token == ")")
                        {
                            scan();
                        }
                        else
                        {
                            outputTextbox.Text += "Chyba na riadku " + lineNumber + ": funkcia odpocitaj nieje ukoncena znakom )\n";
                            compilationSucess = false;
                        }
                    }
                    else
                    {
                        outputTextbox.Text += "Chyba na riadku " + lineNumber + ": nepoznam premennu " + variableName + "\n";
                        compilationSucess = false;
                    }
                }
                else if (token == "nasob")
                {
                    scan();
                    if (token == "(")
                    {
                        scan();
                    }
                    else
                    {
                        outputTextbox.Text += "Chyba na riadku " + lineNumber + ": po prikaze nasob nasleduje znak ( \n";
                        compilationSucess = false;
                    }
                    string variableName = token;
                    if (variablesInt.ContainsKey(variableName))
                    {
                        scan();
                        if (token != ",")
                        {
                            outputTextbox.Text += "Chyba na riadku " + lineNumber + ": funkcia nasob vyzaduje 2 argumenty \n";
                            compilationSucess = false;
                        }
                        scan();
                        result.add(new MultiplyInt(variableName, express().evaluate(), variablesInt));
                        if (token == ")")
                        {
                            scan();
                        }
                        else
                        {
                            outputTextbox.Text += "Chyba na riadku " + lineNumber + ": funkcia nasob nieje ukoncena znakom )\n";
                            compilationSucess = false;
                        }
                    }
                    else
                    {
                        outputTextbox.Text += "Chyba na riadku " + lineNumber + ": nepoznam premennu " + variableName + "\n";
                        compilationSucess = false;
                    }
                }
                else if (token == "vydel")
                {
                    scan();
                    if (token == "(")
                    {
                        scan();
                    }
                    else
                    {
                        outputTextbox.Text += "Chyba na riadku " + lineNumber + ": po prikaze vydel nasleduje znak ( \n";
                        compilationSucess = false;
                    }
                    string variableName = token;
                    if (variablesInt.ContainsKey(variableName))
                    {
                        scan();
                        if (token != ",")
                        {
                            outputTextbox.Text += "Chyba na riadku " + lineNumber + ": funkcia vydel vyzaduje 2 argumenty \n";
                            compilationSucess = false;
                        }
                        scan();
                        result.add(new DivideInt(variableName, express().evaluate(), variablesInt));
                        if (token == ")")
                        {
                            scan();
                        }
                        else
                        {
                            outputTextbox.Text += "Chyba na riadku " + lineNumber + ": funkcia vydel nieje ukoncena znakom )\n";
                            compilationSucess = false;
                        }
                    }
                    else
                    {
                        outputTextbox.Text += "Chyba na riadku " + lineNumber + ": nepoznam premennu " + variableName + "\n";
                        compilationSucess = false;
                    }
                }
                else if (token == "cislo")
                {
                    scan();
                    String variableName = token;
                    if (variableNames.Contains(variableName))
                    {
                        outputTextbox.Text += "Chyba na riadku " + lineNumber + ": premenna " + variableName + " uz je zadefinovana \n";
                        compilationSucess = false;
                    }
                    else
                    {
                        scan();
                        if (token == "=")
                        {
                            scan();
                        }
                        else
                        {
                            outputTextbox.Text += "Chyba na riadku " + lineNumber + ": nepoznam prikaz " + token + "\n";
                            compilationSucess = false;
                        }
                        variableNames.Add(variableName);
                        result.add(new AssignInt(variableName, express(), variablesInt));
                    }
                }
                else if (token == "text")
                {
                    scan();
                    String variableName = token;
                    if (variableNames.Contains(variableName))
                    {
                        outputTextbox.Text += "Chyba na riadku " + lineNumber + ": premenna " + variableName + " uz je zadefinovana \n";
                        compilationSucess = false;
                    }
                    else
                    {
                        scan();
                        if (token == "=")
                        {
                            variableNames.Add(variableName);
                            result.add(new AssignString(variableName, scanAllTextUntilNextLine(), variablesString));
                            scan();
                        }
                        else
                        {
                            outputTextbox.Text += "Chyba na riadku " + lineNumber + ": nepoznam prikaz " + token + "\n";
                            compilationSucess = false;
                        }
                        
                    }
                }
                else if (token == "zvuk")
                {
                    scan();
                    String variableName = token;
                    if (variableNames.Contains(variableName))
                    {
                        outputTextbox.Text += "Chyba na riadku " + lineNumber + ": premenna " + variableName + " uz je zadefinovana \n";
                        compilationSucess = false;
                    }
                    else
                    {
                        scan();
                        if (token == "=")
                        {
                            scan();
                            variableNames.Add(variableName);
                            if (sounds.ContainsKey(token))
                            {
                                result.add(new AssignSound(variableName,new Sound(token, sounds[token]), variablesSound));
                                scan();
                            }
                            else
                            {
                                outputTextbox.Text += "Chyba na riadku " + lineNumber + ": nepoznam zvuk " + token + "\n";
                                compilationSucess = false;
                                scan();
                            }
                        }
                        else
                        {
                            outputTextbox.Text += "Chyba na riadku " + lineNumber + ": nepoznam prikaz " + token + "\n";
                            compilationSucess = false;
                        }

                    }
                }
                else if (token == "vypis")
                {
                    scan();
                    if (token == "(")
                    {
                        scan();
                    }
                    else
                    {
                        outputTextbox.Text += "Chyba na riadku " + lineNumber + ": po prikaze vypis nasleduje znak ( \n";
                        compilationSucess = false;
                    }
                    if (variableNames.Contains(token))
                    {
                        result.add(new Vypis(token, variablesInt, variablesString,variablesSound, outputTextbox));
                        scan();
                        if (token == ")")
                        {
                            scan();
                        }
                        else
                        {
                            outputTextbox.Text += "Chyba na riadku " + lineNumber + ": funkcia vypis nieje ukoncena znakom ) \n";
                            compilationSucess = false;
                        }
                    }
                    else
                    {
                        outputTextbox.Text += "Chyba na riadku " + lineNumber + ": nepoznam premennu " + token + "\n";
                        compilationSucess = false;
                    }
                }
                else if (variableNames.Contains(token))
                {
                    string variableName = token;
                    scan();
                    if (token == "=")
                    {
                        if (variablesInt.ContainsKey(variableName))
                        {
                            scan();
                            result.add(new AssignInt(variableName, express(), variablesInt));
                        }
                        else if (variablesSound.ContainsKey(variableName))
                        {
                            scan();
                            result.add(new AssignSound(variableName, new Sound(token, sounds[token]), variablesSound));
                            scan();
                        }
                            
                        else
                        {
                            result.add(new AssignString(variableName, scanAllTextUntilNextLine(), variablesString));
                            scan();
                        }
                    }
                    else
                    {
                        if (procedures.ContainsKey(variableName))
                        {
                            result.add(new Procedure(variableName, procedures[variableName]));
                        }
                        else
                        {
                            outputTextbox.Text += "Chyba na riadku " + lineNumber + ": nepoznam prikaz " + token + "\n";
                            compilationSucess = false;
                        }
                        
                    }

                }
                else if (token == "inak" || token == "koniec")
                {
                    return result;
                }
                else
                {
                    outputTextbox.Text += "Chyba na riadku " + lineNumber + ": nepoznam prikaz " + token + "\n";
                    compilationSucess = false;
                    scan();
                }

            }
            return result;
        }

        private Commands addWhileInt()
        {
            Expression test1 = addSub();
            if (test1 == null)
            {
                outputTextbox.Text += "Chyba na riadku " + lineNumber + ": zadajte ciselny parameter \n";
                compilationSucess = false;
                return null;
            }
            string conditionOperator = token;
            scan();
            Expression test2 = addSub();
            if (test2 == null)
            {
                outputTextbox.Text += "Chyba na riadku " + lineNumber + ": zadajte ciselny parameter \n";
                compilationSucess = false;
                return null;
            }
            if (token == ")")
            {
                scan();
            }
            else
            {
                outputTextbox.Text += "Chyba na riadku " + lineNumber + ": prikaz kym je treba ukoncit znakom ) \n";
                compilationSucess = false;
                return null;
            }
            Command body = compile();
            if (token == "koniec")
            {
                scan();
                return new WhileInt(test1, test2, conditionOperator, body);
            }
            else
            {
                outputTextbox.Text += "Chyba na riadku " + lineNumber + ": prikaz kym musi obsahovat klucove slovo koniec \n";
                compilationSucess = false;
                return null;
            }
        }
        private Commands addWhileString()
        {
            ExpressionStr test1 = scanAllTextUntilEquals();
            if (token == "=" || token == "<>" || token == "><")
            {
                string conditionOperator = token;
                scan();
                ExpressionStr test2 = scanAllTextUntilBrace(); //pridat chybovy vypis      
                if (token == ")")
                {
                    scan();
                }
                else
                {
                    outputTextbox.Text += "Chyba na riadku " + lineNumber + ": prikaz kym je treba ukoncit znakom ) \n";
                    compilationSucess = false;
                    return null;
                }
                Command body = compile();
                if (token == "koniec")
                {
                    scan();
                    return new WhileString(test1, test2, conditionOperator,variablesString, body);
                }
                else
                {
                    outputTextbox.Text += "Chyba na riadku " + lineNumber + ": prikaz otazka musi obsahovat klucove slovo koniec \n";
                    compilationSucess = false;
                    return null;
                }
            }
            else
            {
                outputTextbox.Text += "Chyba na riadku " + lineNumber + ": ocakaval som znak =, <> alebo >< \n";
                compilationSucess = false;
                return null;
            }
        }
        private Commands addConditionString()
        {
            ExpressionStr argument1 = scanAllTextUntilEquals();
            if (token == "=" || token == "<>" || token == "><")
            {
                string conditionOperator = token;
                scan();
                ExpressionStr argument2 = scanAllTextUntilBrace(); //pridat chybovy vypis   
                scan();
                if (token == ")")
                {
                    scan();
                }
                else
                {
                    outputTextbox.Text += "Chyba na riadku " + lineNumber + ": prikaz ak je treba ukoncit znakom ) \n";
                    compilationSucess = false;
                    return null;
                }
                Command isTrue = compile();
                if (token == "inak")
                {
                    scan();
                    Command isFalse = compile();
                    if (token == "koniec")
                    {
                        scan();
                        return new ConditionString(argument1, argument2, conditionOperator, isTrue, isFalse);

                    }
                    else
                    {
                        outputTextbox.Text += "Chyba na riadku " + lineNumber + ": prikaz otazka musi obsahovat klucove slovo koniec \n";
                        compilationSucess = false;
                        return null;
                    }
                }
                else
                {
                    outputTextbox.Text += "Chyba na riadku " + lineNumber + ": prikaz otazka musi obsahovat klucove slovo inak \n";
                    compilationSucess = false;
                    return null;
                }
            }
            else
            {
                outputTextbox.Text += "Chyba na riadku " + lineNumber + ": ocakaval som znak =, <> alebo >< \n";
                compilationSucess = false;
                return null;
            }
        }

        private ConditionInt addConditionInt()
        {
            Expression argument1 = addSub();
            if (argument1 == null)
            {
                outputTextbox.Text += "Chyba na riadku " + lineNumber + ": zadajte ciselny parameter \n";
                compilationSucess = false;
                return null;
            }
            string conditionOperator = token;
            scan();
            Expression argument2 = addSub();
            if (argument2 == null)
            {
                outputTextbox.Text += "Chyba na riadku " + lineNumber + ": zadajte ciselny parameter \n";
                compilationSucess = false;
                return null;
            }
            if (token == ")")
            {
                scan();
            }
            else
            {
                outputTextbox.Text += "Chyba na riadku " + lineNumber + ": prikaz otazka je treba ukoncit znakom ) \n";
                compilationSucess = false;
                return null;
            }
            Command isTrue = compile();
            if (token == "inak")
            {
                scan();
                Command isFalse = compile();
                if (token == "koniec")
                {
                    scan();
                    return new ConditionInt(argument1, argument2, conditionOperator, isTrue, isFalse);

                }
                else
                {
                    outputTextbox.Text += "Chyba na riadku " + lineNumber + ": prikaz otazka musi obsahovat klucove slovo koniec \n";
                    compilationSucess = false;
                    return null;
                }
            }
            else
            {
                outputTextbox.Text += "Chyba na riadku " + lineNumber + ": prikaz otazka musi obsahovat klucove slovo inak \n";
                compilationSucess = false;
                return null;
            }
        }



        private ExpressionStr scanAllTextUntilEquals()
        {
            ExpressionStr result = null;
            string temp = "";
            if (variablesString.ContainsKey(token))
            {
                result = new AcessString(token, variablesString);
                scan();
            }
            else
            {
                while (!(token == "=" || token == "<>" || token == "><"))
                {
                    temp += token + " ";
                    scan();
                }
                if (temp[temp.Length - 1] == ' ')
                {
                    temp.Remove(temp.Length - 1);
                }
                result = new ConstStr(temp);
            }
            return result;
        }

        private ExpressionStr scanAllTextUntilNextLine()
        {
            ExpressionStr result = null;
            if (variablesString.ContainsKey(token.Trim()))
            {
                result = new AcessString(token, variablesString);
            }
            else
            {
                scanForEol();
                result = new ConstStr(token);
            }
            return result;
     }

        private ExpressionStr scanAllTextUntilBrace()
        {
            ExpressionStr result = null;
            string temp = "";
            if (variablesString.ContainsKey(token))
            {
                result = new AcessString(token, variablesString);
            }
            else
            {
                while (token != ")")
                {
                    temp += token + " ";
                    scan();
                }
                if (temp[temp.Length - 1] == ' ')
                {
                    temp.Remove(temp.Length - 1);
                }
                result = new ConstStr(temp);
            }
            return result;
        }

        
        private Expression express()
        {
            Expression result = addSub();
            while (token == "+" || token == "-")
            {
                if (token == "+")
                {
                    scan();
                    result = new Add(result, mulDiv());
                }
                else if (token == "-")
                {
                    scan();
                    result = new Sub(result, mulDiv());
                }
            }
            return result;
        }

        private Expression addSub()
        {
            Expression result = mulDiv();
            while (token == "+"|| token == "-")
            {
                if (token == "+")
                {
                    scan();
                    result = new Add(result, mulDiv());
                }
                else if (token == "-")
                {
                    scan();
                    result = new Sub(result, mulDiv());
                }
            }
            return result;
        }

        private Expression mulDiv()
        {
            Expression result = operand();
            while (token == "*" || token == "/")
            {
                if (token == "*")
                {
                    scan();
                    result = new Mul(result, operand());
                }
                else if (token == "/")
                {
                    scan();
                    result = new Div(result, operand());
                }
            }
            return result;
        }

        private Expression operand()
        {
            Expression result = null;
            if (kind == NUMBER)
            {
                result = new ConstInt(Convert.ToInt32(token));
                scan();
            }
            else if (kind == WORD)
            {
                if (variablesInt.ContainsKey(token))
                {
                    result = new AccessInt(token, variablesInt);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("nepoznam prikaz " + token);
                }
                scan();
            }
            else if (token == "(")
            {
                scan();
                result = express();
                if (token != ")")
                {
                    System.Diagnostics.Debug.WriteLine("chyba ')' ");
                }
                scan();
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("nepoznam prikaz " + token);
                result = null;
                return result;
            }
            return result;   
        }
        private void scan()
        {
            token = "";
            kind = NOTHING;
            while ((kind == NOTHING) && (look != '\0'))
            {
                if (look == ' ' || look == '\n' || look == '\r')
                {
                    if (look == '\n')
                    {
                        lineNumber++;
                    }
                    next();
                }
                else if (char.IsDigit(look))
                {
                    do
                    {
                        token += look;
                        next();
                    }
                    while (char.IsDigit(look));
                    if (look == '.')
                    {
                        do
                        {
                            token += look;
                            next();
                        }
                        while (char.IsDigit(look));
                    }
                    kind = NUMBER;
                }
                else if (char.IsLetter(look))
                {
                    do
                    {
                        token += look;
                        next();
                    }
                    while (char.IsLetter(look) || char.IsDigit(look));
                    kind = WORD;
                }
                else if (look == '/')
                {

                    next();
                    if (look == '/')
                    {
                        while (!(look == '\0' || look == '\n'))
                        {
                            next();
                        }
                    }
                    else
                    {
                        token = "/";
                        kind = SYMBOL;
                    }
                }
                else if (look == '<' | look == '>' | look == '=')
                {
                    token = look.ToString();
                    next();
                    if (look == '=')
                    {
                        token = token + '=';
                        next();
                    }
                    else if (look == '<')
                    {
                        token = token + '<';
                        next();
                    }
                    else if (look == '>')
                    {
                        token = token + '>';
                        next();
                    }
                    kind = SYMBOL;
                }
                else
                {
                    token = look.ToString();
                    next();
                    kind = SYMBOL;
                }
            }
        }
        private void scanForEol()
        {
            token = "";
            kind = NOTHING;
            while (look != '\n')
            {
                token += look;
                next();
            }
        }
        private void next()
        {
            if (index >= programText.Length)
            {
                look = '\0';
            }
            else
            {
                look = programText[index];
                index++;
            }
        }
    }
}
