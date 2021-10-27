using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse.SimulationSandbox
{
    public class FileSoundEffect : ISoundEffect
    {
        private static NetCoreAudio.Player player = new NetCoreAudio.Player();

        private string _filePath;

        public FileSoundEffect(string filePath)
        {
            _filePath = filePath;
        }

        public void Play()
        {
            player.Play(_filePath).ConfigureAwait(false).GetAwaiter().GetResult();
            System.Threading.Thread.Sleep(500);
        }
    }
}
