namespace Multiverse
{
    public interface IWorld
    {
        int Id { get; set; }

        string Universe { get; set; }

        ulong Timestamp { get; set; }
    }
}
