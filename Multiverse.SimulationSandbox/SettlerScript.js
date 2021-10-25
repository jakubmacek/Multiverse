"use strict";
/// <reference path="multiverse.d.ts" />
debugging.log('script compiles ok');
function onevent(self, event) {
    //debugging.log('event ' + event.type + ' happened at ' + event.timestamp);
    //const forest = scanning.scanAround(self).filter(x => x.type === Forest).shift();
    //if (forest) {
    //    if (forest.x === self.x && forest.y === self.y) {
    //        const harvestresult = abilities.use(self, 'HarvestWood', forest);
    //        debugging.log('harvest wood: ' + harvestresult.type);
    //    } else {
    //        const movementability = self.abilities.withType(AbilityTypeMovement).shift();
    //        const moveresult = abilities.use(self, movementability.name, null, forest.x, forest.y);
    //        debugging.log('move to forest: ' + moveresult.type);
    //    }
    //}
    var forest = scanning.scanAround(self).filter(function (x) { return x.type === Forest; }).shift();
    var buildingSite = scanning.scanAround(self).filter(function (x) { return x.type === WarehouseBuildingSite; }).shift();
    var warehouse = scanning.scanAround(self).filter(function (x) { return x.type === Warehouse; }).shift();
    var t = event.timestamp;
    if (forest && warehouse) {
        abilities.use(self, 'HarvestWood', forest);
        abilities.transferResource(self, warehouse, Wood, 1000);
    }
    if (t === 1) {
        abilities.use(self, 'Move', null, forest.x, forest.y);
    }
    if (t === 2) {
        abilities.startBuildingSite(self, WarehouseBuildingSite);
    }
    if (t === 3) {
        abilities.use(self, 'HarvestWood', forest);
        abilities.use(self, 'HarvestWood', forest);
        abilities.use(self, 'HarvestWood', forest);
        abilities.transferResource(self, buildingSite, Wood, 1000);
    }
    if (t === 4) {
        abilities.use(self, 'HarvestWood', forest);
        abilities.use(self, 'HarvestWood', forest);
        abilities.use(self, 'HarvestWood', forest);
        abilities.transferResource(self, buildingSite, Wood, 1000);
    }
    if (t === 5) {
        abilities.build(self, buildingSite);
    }
    if (t === 6) {
        abilities.build(self, buildingSite);
    }
    return null;
}
