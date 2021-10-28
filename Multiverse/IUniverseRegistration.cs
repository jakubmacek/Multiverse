namespace Multiverse
{
    public interface IUniverseRegistration
    {
        string Name { get; }

        IUniverseFactory Factory { get; }
    }
}