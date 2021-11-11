using System;
using System.Collections.Generic;

namespace Multiverse
{
    public class SoundEffects : ISoundEffects
    {
        protected Dictionary<string, ISoundEffect> _soundEffects = new();

        public ISoundEffect this[string name]
        {
            get
            {
                if (!_soundEffects.TryGetValue(name, out ISoundEffect? value))
                    _soundEffects[name] = value = new DummySoundEffect(name, null/*Console.Out*/);
                return value;
            }
            set
            {
                _soundEffects[name] = value;
            }
        }
    }
}
