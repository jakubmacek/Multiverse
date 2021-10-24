using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse.SimpleUniverse
{
    public sealed class SimpleUniverse : Universe
    {
        public static readonly Guid GaiaPlayerId = Guid.Empty;

        public SimpleUniverse(IRepositoryFactoryFactory repositoryFactoryFactory, int worldId)
            : base(repositoryFactoryFactory, worldId, R.All)
        {
            ScriptingEngineFactory.AddLibrary(new Scripting.Constants(this));
            ScriptingEngineFactory.AddLibrary(new Scripting.Debugging(this));
        }

        protected override void EnumerateUnits(IUnitTypeVisitor v)
        {
            v.Visit<Forest>();
            v.Visit<Settler>();
        }

        public override void EnsureInitialWorldState()
        {
            base.EnsureInitialWorldState();

            var gaia = new Player() { Id = GaiaPlayerId, Name = "Gaia" };
            if (Repository.GetPlayer(gaia.Id) == null)
                Repository.Save(gaia);
        }

        public override Unit CreateUnit<T>(Player player, Place place)
        {
            var typeOfT = typeof(T);
            if (typeOfT == typeof(Settler))
                return new Settler();
            else if (typeOfT == typeof(Forest))
                return new Forest();
            else
                throw new ArgumentException("This unit type is not known in the universe.");
        }
    }
}
