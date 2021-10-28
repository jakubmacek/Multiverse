namespace Multiverse
{
    public class UniverseRegistration : IUniverseRegistration
    {
        public string Name { get; init; }

        public IUniverseFactory Factory { get; init; }

        public UniverseRegistration(string name, IUniverseFactory factory)
        {
            Name = name;
            Factory = factory;
        }
    }
}
