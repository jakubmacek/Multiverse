namespace Multiverse.Server
{
    public class WorldResource
    {
        public int Id { get; init; }

        public string ConstantName { get; init; }

        public string Name { get; init; }

        public bool FreelyTransferrable { get; init; }

        public WorldResource(Resource resource)
        {
            Id = resource.Id;
            ConstantName = resource.ConstantName;
            Name = resource.Name;
            FreelyTransferrable = resource.FreelyTransferrable;
        }
    }
}
