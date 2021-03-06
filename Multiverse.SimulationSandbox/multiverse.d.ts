declare const Wood: number;
declare const BuildingWork: number;

declare const Forest: string;
declare const Settler: string;
declare const Warehouse: string;
declare const WarehouseBuildingSite: string;

declare const AbilityTypeOther: number;
declare const AbilityTypeMovement: number;
declare const AbilityTypeAttack: number;
declare const AbilityTypeUnitCreation: number;
declare const AbilityTypeResourceGathering: number;

declare const debugging: {
    log(message: string): void
    error(message: string): void
};

interface ScriptingEvent {
    timestamp: number;
    type: string;
}

interface UnitResources {
    [id: number]: number;
}

interface UnitCapacities {
    [id: number]: number;
}

interface UnitAbility {
    name: string;
    type: number;
    cooldownTime: number;
    cooldownTimestamp: number;
    maxAvailableUses: number;
    remainingUses: number;
    usesRestoredOnCooldown: number;
    actionPointCost: number;
}

interface UnitAbilities {
    length: number;
    [offset: number]: UnitAbility;
    withType(type: number): UnitAbility[];
}

interface Unit {
    id: string;
    type: string;
    playerId: number;
    name: string;
    x: number;
    y: number;
    indestructible?: boolean;
    health?: number;
    maxHealth?: number;
    immovable?: boolean;
    movementPoints?: number;
    maxMovementPoints?: number;
    actionPoints?: number;
    maxActionPoints?: number;
    capacities?: UnitCapacities;
    resources?: UnitResources;
    abilities?: UnitAbilities;
}

interface UnitSelf extends Unit {
    playerData?: string;
}

declare const scanning: {
    scanAround(self: UnitSelf): Unit[];
    scanUnit(self: UnitSelf, target: Unit): Unit;
}

interface UnitAbilityUseResult {
    type: string;
    amount: number;
}

declare const abilities: {
    use(self: UnitSelf, abilityName: string): UnitAbilityUseResult
    use(self: UnitSelf, abilityName: string, targetUnit: Unit): UnitAbilityUseResult
    use(self: UnitSelf, abilityName: string, targetUnit: Unit, targetPlaceX: number, targetPlaceY: number): UnitAbilityUseResult

    startBuildingSite(self: UnitSelf, buildingSiteName: string): UnitAbilityUseResult;
    build(self: UnitSelf, targetBuildingSite: Unit): UnitAbilityUseResult;
    transferResource(self: UnitSelf, target: Unit, resourceId: number, amount: number): UnitAbilityUseResult;
}

declare const battle: {
    start(self: UnitSelf, target: Unit): UnitAbilityUseResult;
}

interface UnitBattleAbility extends UnitAbility {

}

interface Battle {
    participants: Unit[];
}

interface BattleEvent {
    type: string;
}

interface BattleStartEvent extends BattleEvent {
    addParticipant(unit: Unit): string;
}

interface BattleRoundEvent extends BattleEvent {
    round: number;
}

interface BattleEndEvent extends BattleEvent {
}
