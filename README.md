# Multiverse game

The goal is to create an online multiplayer strategy game where players do not control units directly, but prepare AI scripts and assign them to the units. I have originally tried to implement this using ApplicationDomains way back in 2006 when C# was getting its 2nd version. But while the MMO part worked fairly OK, there were not good scripting options.

I programmed the reimagined Multiverse over a weekend using .NET 5 and NLua for scripting and I was very happy on Sunday that the simulation worked as I intended on Friday. :-)

Currently, there is a single, very simple, universe programmed. If you want a similar, but complete game, search for *Screeps*.

## Rules of the game

* Universe defines sets of units (and buildings), resources, actions and rules in general.
* Multiple isolated worlds can exist within one universe.
* Time is discrete, starts at 0, each "tick" gives every unit the opportunity to react.
* Units have available actions. Actions are different for each unit type (possibly for each individual unit). Actions are processed immediately when used in a script. More than one action can be done during one tick.

## TO-DO SimpleUniverse

* Think of some basic measure of success - perhaps the player with most wood is the best?
* Create a storage building. Implement "create building site", "build" and "store" actions. The building site is placed and cannot be moved, has 1 HP (is vulnerable). According to a selected building, some resources (including "building work" resource) are needed. Bulding site will have in-game script force-assigned. Building takes multiple ticks to build. Settlers/workers have to drop off material and provide work using "build" action. Then enough resources and work has been done, site's tick counter increases and part of the resources is comsumed.
* Implement attack related actions.

## TO-DO general

* Implement the PostgreSQL storage and prepare the initial and resetting SQL migrations.
* Implement the Web application:
  * Simulation - regular ticks for all running worlds.
  * User registration, sign in, sign out. User may have multiple player accounts.
  * Administration of worlds - create new, delete.
  * World map view, unit view with list of their resources, abilities, etc. This is fairly necessary for debugging. Maybe use Vue.js / React, maybe dynamically generated SVG for the map.
  * Player script management - create new script, assign to units, show log, send message to a unit (i.e. an order).
* Write documentation with script examples.
* Possibly implement JavaScript (TypeScript) as an alternative scripting language. It should be easier to write TS scripts, but Lua is probably more efficient.
* Implement turn-based battles where all participating units have to declare handler functions in their script.
* Implement communication between units. Should the unit receive communication immediately or during the next tick? Should distance be limited? This will affect the scenario where individual units are fairly dumb, but they obey commands from their commanding unit.
* Implement scripting libraries - especially pathfinding.
* Unit deaths. Unit with HP <= 0 will not be able to handle events. The records need to be cleaned up after a while.
