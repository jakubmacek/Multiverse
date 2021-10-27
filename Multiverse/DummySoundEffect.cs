using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse
{
    public class DummySoundEffect : ISoundEffect
    {
        public string Name { get; init; }
        public TextWriter? WriteWhere { get; init; }

        public DummySoundEffect(string name, TextWriter? writeWhere)
        {
            Name = name;
            WriteWhere = writeWhere;
        }

        public void Play()
        {
            if (WriteWhere != null)
                WriteWhere.WriteLine($"Sound effect: {Name}.");
        }
    }
}
