using System.Text.Json.Serialization;

namespace Multiverse.Server.Authorization
{
    public struct UserPlayer
    {
        public int PlayerId { get; init; }

        public UserPlayer(int playerId)
        {
            PlayerId = playerId;
        }
    }
}
