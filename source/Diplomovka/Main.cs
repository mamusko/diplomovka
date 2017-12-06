using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using AutocompleteMenuNS;
using System.Reflection;
using System.IO;

namespace Diplomovka
{
    //www.codeproject.com/Articles/365974/Autocomplete-Menu
    //commit test
    public partial class Main : Form
    {
        List<string> commands = new List<string>();

        public Main()
        {
            InitializeComponent();
            initializeCommands();
            initializeSounds();
            initializeMenuWCommands();
            
            string pripocitaj = "cislo pocet = 4 \n" +
                "pripocitaj(pocet,6) \n" +
                "pripocitaj(pocet,500)\n ";
            string textPodmienkyVCykle = "text meno = ahoj \n" +
                "cislo pocet = 3 \n" +
                "opakuj pocet krat \n" +
                "otazka (ahoj) je ano \n" +
                "zahraj(ano) \n" +
                "inak \n" +
                "zahraj(nie) koniec\n" +
                " koniec";
            string podmienka = "text meno = matus je krasny \n" +
                "text priezvisko = kovac\n" +
                "ak (matus = meno)\n" +
                "zahraj(rovnajuSa)\n" +
                "inak\n" +
                "zahraj(nerovnajuSa)\n" +
                "koniec\n";
            string kym = "" +
                "cislo pocet = 5\n" +
                "kym (pocet >= 0)\n" +
                "vypis(pocet)\n" +
                "pocet = pocet - 1\n" +
                "koniec";
            string kym2 = "" +
                "text meno = matusko je krasny\n" +
                "kym (meno >< matus)\n" +
                "vypis(meno)\n" +
                "meno = matus\n" +
                "koniec";
            string procedureRepeat = "urob zahrajMiNieco \n" +
                "zahraj(nieco)\n" +
                "cislo pocet = 4\n" +
                "opakuj pocet krat \n" +
                "zahraj(nieco2) \n" +
                "koniec\n" +
                "koniec\n" +
                "zahrajMiNieco\n" +
                "zahrajMiNieco ";
            string procedure = "urob zahrajMiNieco \n" +
                "zahraj(nieco)\n" +
                "zahraj(niecovopakuj) \n" +
                "koniec\n" +
                "zahrajMiNieco\n";
            string podlaNavrhu = "" +
                "opakuj 10 krat\n" +
                "povedz(nebudem podvadzat na teste)\n" +
                "koniec\n" +
                "cislo pocetOpakovani = 4\n" +
                "opakuj pocetOpakovani krat\n" +
                "vypis(pocetOpakovani)\n" +
                "koniec\n" +
                "ak (pocetOpakovani <= 0)\n";

            string premennazvuk =
                "Zvuk sound = nieco\n" +
                "sound = nieco2\n" +
                "vypis(sound)\n" +
                "zahraj(sound)  ";
            string premenntatext =
                "text krstne = matus \n" +
                "text priezvisko = kovac \n" +
                "krstne = priezvisko\n" +
                "vypis (priezvisko)";
            richTextBox1.Text = premenntatext;
            // todo nedaju sa pouzit cisla v nazvoch premennych edit uz by sa mali dat ale neni to otestovane
            
        }

        private void initializeCommands()
        {
            commands.Add("zahraj");
            commands.Add("povedz");
            commands.Add("opakuj");
            commands.Add("krat");
            commands.Add("koniec");
            commands.Add("otazka");
            commands.Add("je");
            commands.Add("ano");
            commands.Add("inak");
            commands.Add("ak");
            commands.Add("kym");
            commands.Add("urob");
            commands.Add("pripocitaj");
            commands.Add("odpocitaj");
            commands.Add("nasob");
            commands.Add("vydel");
            commands.Add("cislo");
            commands.Add("text");
            commands.Add("vypis");
        }

        private void initializeMenuWCommands()
        {
            AutocompleteItem item;
            foreach (string command in commands)
            {
                item = new AutocompleteItem(command);
                item.MenuText = command;
                autocompleteMenu1.AddItem(item);
            }

            //item.MenuText = "zahraj(zvuk)";
            //autocompleteMenu1.AddItem(item);

            //item = new AutocompleteItem("povedz");
            //item.MenuText = "povedz(text)";
            //autocompleteMenu1.AddItem(item);
        }

        private void initializeSounds()
        {
            string currentDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            string route = Path.Combine(currentDirectory, "Zvuky\\");
            string[] folderNames = Directory.GetDirectories(route);
            for (int i = 0; i < folderNames.Length; i++)
            {
                string[] soundNames = Directory.GetFiles(folderNames[i], "*.wav");
                for (int j = 0; j < soundNames.Length; j++)
                {
                    commands.Add(trimSoundName(soundNames[j]));
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
        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox2.Text = "";
            Compiler compiler = new Compiler(richTextBox1, richTextBox2);
            Command program = compiler.compile();
            if (compiler.compilationSucess)
            {
                richTextBox2.Text += "Kompilacia prebehla uspesne\n";
                program.execute();
            }
            else
            {
                richTextBox2.Text += "Vytvoreny program nieje mozne spustit, opravte vsetky chyby";
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (richTextBox1.Text == "")
            {
                addLineNumbers();
            }
        }


        public int getWidth()
        {
            int w = 25;
            int line = richTextBox1.Lines.Length;

            if (line <= 99)
            {
                w = 20 + (int)richTextBox1.Font.Size;
            }
            else if (line <= 999)
            {
                w = 30 + (int)richTextBox1.Font.Size;
            }
            else
            {
                w = 50 + (int)richTextBox1.Font.Size;
            }

            return w;
        }

        public void addLineNumbers()
        {
            Point pt = new Point(0, 0);
            int firstIndex = richTextBox1.GetCharIndexFromPosition(pt);
            int firstLine = richTextBox1.GetLineFromCharIndex(firstIndex);
            pt.X = ClientRectangle.Width;
            pt.Y = ClientRectangle.Height;
            int lastIndex = richTextBox1.GetCharIndexFromPosition(pt);
            int lastLine = richTextBox1.GetLineFromCharIndex(lastIndex);
            richTextBox3.SelectionAlignment = HorizontalAlignment.Center;
            richTextBox3.Text = "";
            richTextBox3.Width = getWidth();
            for (int i = firstLine; i <= lastLine + 2; i++)
            {
                richTextBox3.Text += i + 1 + "\n";
            }
        }

        private void Main_Load(object sender, EventArgs e)
        {
            richTextBox3.Font = richTextBox1.Font;
            richTextBox1.Select();
            addLineNumbers();
        }

        private void Main_Resize(object sender, EventArgs e)
        {
            addLineNumbers();
        }

        private void richTextBox1_SelectionChanged(object sender, EventArgs e)
        {
            Point pt = richTextBox1.GetPositionFromCharIndex(richTextBox1.SelectionStart);
            if (pt.X == 1)
            {
                addLineNumbers();
            }

        }

        private void richTextBox1_VScroll(object sender, EventArgs e)
        {
            richTextBox3.Text = "";
            addLineNumbers();
            richTextBox3.Invalidate();
        }

        private void richTextBox1_FontChanged(object sender, EventArgs e)
        {
            richTextBox3.Font = richTextBox1.Font;
            richTextBox1.Select();
            addLineNumbers();
        }

        private void richTextBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {   
                ContextMenu contextMenu = new System.Windows.Forms.ContextMenu();
                MenuItem menuItem = new MenuItem("Cut");
                menuItem.Click += new EventHandler(CutAction);
                contextMenu.MenuItems.Add(menuItem);
                menuItem = new MenuItem("Copy");
                menuItem.Click += new EventHandler(CopyAction);
                contextMenu.MenuItems.Add(menuItem);
                menuItem = new MenuItem("Paste");
                menuItem.Click += new EventHandler(PasteAction);
                contextMenu.MenuItems.Add(menuItem);

                richTextBox1.ContextMenu = contextMenu;
            }
        }
        void CutAction(object sender, EventArgs e)
        {
            richTextBox1.Cut();
        }

        void CopyAction(object sender, EventArgs e)
        {
            Clipboard.SetText(richTextBox1.SelectedText);
        }

        void PasteAction(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                richTextBox1.Text
                    += Clipboard.GetText(TextDataFormat.Text).ToString();
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Tab)
            {
                richTextBox1.Select(richTextBox1.SelectionStart, 0);
                richTextBox1.SelectedText = "   ";
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
