using System;

namespace Multiverse.Server
{
    public class RunningWorld
    {
        public int Id => Universe.World.Id;

        public IUniverse Universe { get; init; }

        public DateTime NextTickOn { get; private set; }

        public TimeSpan NextTickAfter { get; } = new TimeSpan(0, 1, 0); // 1 minute

        public RunningWorld(IUniverse universe)
        {
            Universe = universe;
            universe.EnsureInitialWorldState();
            NextTickOn = DateTime.Now.Add(NextTickAfter);
        }

        private void Tick()
        {
            Universe.Tick();
            NextTickOn = DateTime.Now.Add(NextTickAfter);
        }

        public void TickIfItIsTimeTo()
        {
            if (NextTickOn >= DateTime.Now)
                Tick();
        }
    }
}
