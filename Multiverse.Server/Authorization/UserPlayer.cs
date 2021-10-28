using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse.Server.Authorization
{
    public struct UserPlayer
    {
        public readonly int PlayerId;

        public UserPlayer(int playerId)
        {
            PlayerId = playerId;
        }
    }
}
