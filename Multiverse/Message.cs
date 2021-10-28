using System;
using System.Collections.Generic;

namespace Multiverse
{
    public class Message : IMessage
    {
        public virtual Guid Id { get; set; }

        public virtual World? World { get; set; }

        public virtual Player? Player { get; set; }

        public virtual MessageType Type { get; set; }

        public virtual DateTime SentAt { get; set; }

        public virtual long SentAtTimestamp { get; set; }

        public virtual long? ReceivedAtTimestamp { get; set; }

        public virtual Guid? FromUnit { get; set; }

        public virtual Guid? ToUnit { get; set; }

        public virtual string Text { get; set; } = string.Empty;

        public virtual PlayerData PlayerData { get; set; }
    }
}
