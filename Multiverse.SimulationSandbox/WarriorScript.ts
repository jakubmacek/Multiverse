/// <reference path="multiverse.d.ts" />

debugging.log('warrior script compiles ok');

function onevent(self: UnitSelf, event: ScriptingEvent): string {
    const enemies = scanning.scanAround(self).filter(x => x.playerId && x.playerId !== self.playerId && x.health > 0);

    debugging.log(self.name + ' seeing enemies: ' + enemies.length);

    if (enemies.length > 0) {
        battle.start(self, enemies[0]);
    }

    return null;
}

function onbattlestart(self: UnitSelf, event: BattleStartEvent, battle: Battle): string {
    const friends = scanning.scanAround(self).filter(x => x.playerId === self.playerId && x.x === self.x && x.y === self.y);
    const enemies = scanning.scanAround(self).filter(x => x.playerId && x.playerId !== self.playerId && x.x === self.x && x.y === self.y);

    for (const friend of friends) {
        debugging.log(self.name + ' battle start, add friend: ' + friend.name);
        event.addParticipant(friend);
    }

    for (const enemy of enemies) {
        debugging.log(self.name + ' battle start, add enemy: ' + enemy.name);
        event.addParticipant(enemy);
    }

    return null;
}

function onbattleround(self: UnitSelf, event: BattleRoundEvent, battle: Battle): string {
    const enemies = battle.participants.filter(x => x.playerId !== self.playerId);

    for (const enemy of enemies) {
        let scannedenemy = scanning.scanUnit(self, enemy);
        if (scannedenemy.health > 0) {
            const attackresult = abilities.use(self, 'Attack', enemy);
            scannedenemy = scanning.scanUnit(self, enemy);
            debugging.log(self.name + ' attacked ' + enemy.name + ' for ' + attackresult.amount + ' HP; it now has ' + scannedenemy.health + ' HP');
        }
    }

    return null;
}