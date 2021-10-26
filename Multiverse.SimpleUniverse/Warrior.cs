using System.Collections.Generic;

namespace Multiverse.SimpleUniverse
{
    public class Warrior : Unit
    {
        public override bool Indestructible => false;

        public override int MaxHealth => 200;

        public override bool Immovable => false;

        public override int MaxMovementPoints => 1;

        public override int MaxActionPoints => 10;

        public override IScanCapability ScanCapability => ScanCapabilities.SeeGaiaAndOwn1;

        public override IEnumerable<UnitAbility> CreateAbilities()
        {
            yield return new MoveOnLand();
            yield return new WarriorAttack();
        }

        public override int GetResourceCapacity(int resourceId)
        {
            return 0;
        }
    }
}
