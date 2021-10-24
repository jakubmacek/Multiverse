using System;

namespace Multiverse
{
    public interface IScriptingEngine : IDisposable
    {
        public IScript Script { get; }

        public void RegisterObject(string name, object obj);

        public ScriptingRunEventResult RunEvent(Event @event, IUnit unit);
    }
}
