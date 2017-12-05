using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Diplomovka
{
    class Sound
    {
        public string soundName;
        string soundCategory;
        string soundRoute;

        public Sound(string name, string category)
        {
            soundName = name;
            soundCategory = category;
            string currentDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            this.soundRoute = Path.Combine(currentDirectory, "Zvuky\\", soundCategory, soundName);
        }

        public void playSound()
        {
            new SoundPlayer(soundRoute).PlaySync();
        }
    }
}
