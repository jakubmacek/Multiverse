﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse.SimpleUniverse
{
    public sealed class SimpleUniverse : Universe
    {
        public static readonly int GaiaPlayerId = 0;

        public SimpleUniverse(IRepositoryFactoryFactory repositoryFactoryFactory, int worldId)
            : base(repositoryFactoryFactory, worldId, R.All)
        {
            ScriptingEngineFactory.AddLibrary(new Scripting.Constants(this));
            ScriptingEngineFactory.AddLibrary(new Scripting.Debugging(this));
            ScriptingEngineFactory.AddLibrary(new Scripting.Scanning(this));
            ScriptingEngineFactory.AddLibrary(new Scripting.Abilities(this));
        }

        protected override void EnumerateUnits(IUnitTypeVisitor v)
        {
            v.Visit<Forest>();
            v.Visit<Settler>();
            v.Visit<Warehouse>();
            v.Visit<WarehouseBuildingSite>();
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
            else if (typeOfT == typeof(Warehouse))
                return new Warehouse();
            else if (typeOfT == typeof(WarehouseBuildingSite))
                return new WarehouseBuildingSite();
            else
                throw new ArgumentException("This unit type is not known in the universe.");
        }
    }
}
