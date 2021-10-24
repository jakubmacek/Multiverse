﻿using System;

namespace Multiverse
{
    public interface IMessage
    {
        Guid? FromUnit { get; set; }

        Guid Id { get; set; }

        Player? Player { get; set; }

        PlayerData PlayerData { get; set; }

        ulong? ReceivedAtTimestamp { get; set; }

        DateTime SentAt { get; set; }

        ulong SentAtTimestamp { get; set; }

        string Text { get; set; }

        Guid? ToUnit { get; set; }

        MessageType Type { get; set; }

        World? World { get; set; }
    }
}