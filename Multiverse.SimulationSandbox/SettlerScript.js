"use strict";
/// <reference path="multiverse.d.ts" />
debugging.log('script compiles ok');
function onevent(self, event) {
    debugging.log('event ' + event.type + ' happened at ' + event.timestamp);
    var forest = scanning.scanAround(self).filter(function (x) { return x.type === Forest; }).shift();
    if (forest) {
        if (forest.x === self.x && forest.y === self.y) {
            var harvestresult = abilities.use(self, 'HarvestWood', forest);
            debugging.log('harvest wood: ' + harvestresult.type);
        }
        else {
            var movementability = self.abilities.withType(AbilityTypeMovement).shift();
            var moveresult = abilities.use(self, movementability.name, null, forest.x, forest.y);
            debugging.log('move to forest: ' + moveresult.type);
        }
    }
    return null;
}
