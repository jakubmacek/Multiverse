"use strict";
/// <reference path="multiverse.d.ts" />
debugging.log('warrior script compiles ok');
function onevent(self, event) {
    var enemies = scanning.scanAround(self).filter(function (x) { return x.playerId && x.playerId !== self.playerId && x.health > 0; });
    debugging.log(self.name + ' seeing enemies: ' + enemies.length);
    if (enemies.length > 0) {
        battle.start(self, enemies[0]);
    }
    return null;
}
function onbattlestart(self, event, battle) {
    var friends = scanning.scanAround(self).filter(function (x) { return x.playerId === self.playerId && x.x === self.x && x.y === self.y; });
    var enemies = scanning.scanAround(self).filter(function (x) { return x.playerId && x.playerId !== self.playerId && x.x === self.x && x.y === self.y; });
    for (var _i = 0, friends_1 = friends; _i < friends_1.length; _i++) {
        var friend = friends_1[_i];
        debugging.log(self.name + ' battle start, add friend: ' + friend.name);
        event.addParticipant(friend);
    }
    for (var _a = 0, enemies_1 = enemies; _a < enemies_1.length; _a++) {
        var enemy = enemies_1[_a];
        debugging.log(self.name + ' battle start, add enemy: ' + enemy.name);
        event.addParticipant(enemy);
    }
    return null;
}
function onbattleround(self, event, battle) {
    var enemies = battle.participants.filter(function (x) { return x.playerId !== self.playerId; });
    for (var _i = 0, enemies_2 = enemies; _i < enemies_2.length; _i++) {
        var enemy = enemies_2[_i];
        var scannedenemy = scanning.scanUnit(self, enemy);
        if (scannedenemy.health > 0) {
            var attackresult = abilities.use(self, 'Attack', enemy);
            scannedenemy = scanning.scanUnit(self, enemy);
            debugging.log(self.name + ' attacked ' + enemy.name + ' for ' + attackresult.amount + ' HP; it now has ' + scannedenemy.health + ' HP');
        }
    }
    return null;
}
