/// <reference path="multiverse.d.ts" />

debugging.log('script compiles ok');

function onevent(self: UnitSelf, event: ScriptingEvent): string {
    debugging.log('event ' + event.type + ' happened at ' + event.timestamp);
    const forest = scanning.scanAround(self).filter(x => x.type === Forest).shift();
    if (forest) {
        if (forest.x === self.x && forest.y === self.y) {
            const harvestresult = abilities.use(self, 'HarvestWood', forest);
            debugging.log('harvest wood: ' + harvestresult.type);
        } else {
            const movementability = self.abilities.withType(AbilityTypeMovement).shift();
            const moveresult = abilities.use(self, movementability.name, null, forest.x, forest.y);
            debugging.log('move to forest: ' + moveresult.type);
        }
    }

    return null;
}
