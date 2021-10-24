using System;

namespace Multiverse
{
    public interface IScript
    {
        Guid Id { get; set; }

        string? Name { get; set; }

        Player? Player { get; set; }

        string? Source { get; set; }

        ScriptEngineType Type { get; set; }
    }
}