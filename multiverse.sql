-- Adminer 4.17.1 PostgreSQL 13.14 (Debian 13.14-1.pgdg120+2) dump

DROP TABLE IF EXISTS "message";
CREATE TABLE "public"."message" (
    "id" uuid NOT NULL,
    "world" integer NOT NULL,
    "player" integer NOT NULL,
    "type" integer NOT NULL,
    "sentat" timestamp NOT NULL,
    "sentattimestamp" bigint NOT NULL,
    "receivedattimestamp" bigint,
    "fromunit" uuid,
    "tounit" uuid,
    "text" character varying(1000) NOT NULL,
    "playerdatavalue" character varying(1000),
    CONSTRAINT "message_pkey" PRIMARY KEY ("id")
) WITH (oids = false);


DROP TABLE IF EXISTS "player";
CREATE TABLE "public"."player" (
    "id" integer NOT NULL,
    "name" character varying(100) NOT NULL,
    CONSTRAINT "player_pkey" PRIMARY KEY ("id")
) WITH (oids = false);

INSERT INTO "player" ("id", "name") VALUES
(0,	'Gaia'),
(1,	'One'),
(2,	'Two');

DROP TABLE IF EXISTS "script";
CREATE TABLE "public"."script" (
    "id" uuid NOT NULL,
    "player" integer NOT NULL,
    "name" character varying(200) NOT NULL,
    "type" integer NOT NULL,
    "source" text NOT NULL,
    CONSTRAINT "script_pkey" PRIMARY KEY ("id")
) WITH (oids = false);

INSERT INTO "script" ("id", "player", "name", "type", "source") VALUES
('50be27bb-a5da-4c1e-9fe1-c16fdcf45bf8',	1,	'movearound',	2,	'"use strict";
/// <reference path="multiverse.d.ts" />
function onevent(self, event) {
    var t = event.timestamp;
    var x = self.x;
    var y = self.y;
    if ((t % 3) === 1) {
        x--;
    }
    else if ((t % 3) === 0) {
        x++;
    }
    if ((t % 4) === 1) {
        y--;
    }
    else if ((t % 4) === 0) {
        y++;
    }
    debugging.log(self.name + " moves to " + x + ", " + y);
    abilities.use(self, ''Move'', null, x, y);
    return null;
}
'),
('a12a3bfa-45db-46ea-9537-94dbd191dfd9',	1,	'test',	2,	'test');

DROP TABLE IF EXISTS "unit";
CREATE TABLE "public"."unit" (
    "id" uuid NOT NULL,
    "type" character varying(200) NOT NULL,
    "player" integer NOT NULL,
    "world" integer NOT NULL,
    "placex" integer NOT NULL,
    "placey" integer NOT NULL,
    "playerdatavalue" character varying(1000),
    "script" uuid,
    "name" character varying(200) NOT NULL,
    "health" integer NOT NULL,
    "movementpoints" integer NOT NULL,
    "resources" character varying(4000) NOT NULL,
    "abilities" text NOT NULL,
    CONSTRAINT "unit_pkey" PRIMARY KEY ("id")
) WITH (oids = false);

INSERT INTO "unit" ("id", "type", "player", "world", "placex", "placey", "playerdatavalue", "script", "name", "health", "movementpoints", "resources", "abilities") VALUES
('dd8700fd-ef88-47a1-9d5c-a2ecac3c026a',	'Warrior',	1,	1,	1,	2,	NULL,	NULL,	'One''s Warrior 2',	100,	1,	'{}',	'[{"$type":"Multiverse.SimpleUniverse.MoveOnLand, Multiverse.SimpleUniverse","movementRequiredForUse":1,"remainingUses":1,"cooldownTimestamp":1715},{"$type":"Multiverse.SimpleUniverse.WarriorAttack, Multiverse.SimpleUniverse","remainingUses":0,"cooldownTimestamp":0}]'),
('22ce2948-f3aa-4e20-bc70-8d6816858500',	'Warrior',	2,	1,	2,	2,	NULL,	NULL,	'Two''s Warrior 2',	100,	1,	'{}',	'[{"$type":"Multiverse.SimpleUniverse.MoveOnLand, Multiverse.SimpleUniverse","movementRequiredForUse":1,"remainingUses":1,"cooldownTimestamp":1715},{"$type":"Multiverse.SimpleUniverse.WarriorAttack, Multiverse.SimpleUniverse","remainingUses":0,"cooldownTimestamp":0}]'),
('622151f4-d1f8-4bfd-ba18-d34ec1e2b6e6',	'Warrior',	1,	1,	1,	1,	NULL,	NULL,	'One''s Warrior 1',	100,	1,	'{}',	'[{"$type":"Multiverse.SimpleUniverse.MoveOnLand, Multiverse.SimpleUniverse","movementRequiredForUse":1,"remainingUses":1,"cooldownTimestamp":20},{"$type":"Multiverse.SimpleUniverse.WarriorAttack, Multiverse.SimpleUniverse","remainingUses":0,"cooldownTimestamp":0}]'),
('e1459360-12e1-47c3-8caa-ae6d5821dbbf',	'Warrior',	2,	1,	1,	2,	NULL,	NULL,	'Two''s Warrior 1',	100,	1,	'{}',	'[{"$type":"Multiverse.SimpleUniverse.MoveOnLand, Multiverse.SimpleUniverse","movementRequiredForUse":1,"remainingUses":1,"cooldownTimestamp":20},{"$type":"Multiverse.SimpleUniverse.WarriorAttack, Multiverse.SimpleUniverse","remainingUses":0,"cooldownTimestamp":0}]'),
('6b8bb31f-27e0-4b67-92d8-fe450e4e3334',	'Forest',	0,	1,	-1,	0,	NULL,	NULL,	'Forest 1',	0,	0,	'{1:1000}',	'[]'),
('3e4ffb6c-5956-497d-8f32-01b5bce54e82',	'Settler',	1,	1,	0,	0,	NULL,	NULL,	'One''s Settler 1',	100,	1,	'{1:10}',	'[{"$type":"Multiverse.SimpleUniverse.MoveOnLand, Multiverse.SimpleUniverse","movementRequiredForUse":1,"remainingUses":1,"cooldownTimestamp":20},{"$type":"Multiverse.SimpleUniverse.SettlerHarvestWood, Multiverse.SimpleUniverse","harvestedOnUse":10,"remainingUses":5,"cooldownTimestamp":24},{"$type":"Multiverse.SimpleUniverse.SettlerBuildBuilding, Multiverse.SimpleUniverse","buildingWorkPerUse":100,"remainingUses":1,"cooldownTimestamp":22},{"$type":"Multiverse.SimpleUniverse.SettlerStartBuildingSite, Multiverse.SimpleUniverse","availableBuildingSiteTypes":{"$type":"Multiverse.UnitType[], Multiverse","$values":[{"constantName":"WarehouseBuildingSite","name":"Warehouse building site","type":"Multiverse.SimpleUniverse.WarehouseBuildingSite, Multiverse.SimpleUniverse, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"}]},"remainingUses":1,"cooldownTimestamp":22},{"$type":"Multiverse.TransferResource, Multiverse","remainingUses":3,"cooldownTimestamp":22}]');

DROP TABLE IF EXISTS "unitgroup";
CREATE TABLE "public"."unitgroup" (
    "id" uuid NOT NULL,
    "world" integer NOT NULL,
    "player" integer NOT NULL,
    "name" character varying(200) NOT NULL,
    "playerdatavalue" character varying(1000),
    CONSTRAINT "unitgroup_pkey" PRIMARY KEY ("id")
) WITH (oids = false);


DROP TABLE IF EXISTS "unitgroupunits";
CREATE TABLE "public"."unitgroupunits" (
    "unitgroup" uuid NOT NULL,
    "elt" uuid NOT NULL,
    CONSTRAINT "unitgroupunits_pkey" PRIMARY KEY ("unitgroup", "elt")
) WITH (oids = false);


DROP TABLE IF EXISTS "user";
CREATE TABLE "public"."user" (
    "name" character varying(100) NOT NULL,
    "password" character varying(100),
    "role" character varying(50),
    "players" json NOT NULL,
    CONSTRAINT "user_pkey" PRIMARY KEY ("name")
) WITH (oids = false);

INSERT INTO "user" ("name", "password", "role", "players") VALUES
('one',	'AQAAAAEAACcQAAAAEHGkxs4zJNeb3+nl2MsH1Omd7r6jDoUeVr7dlwPUaXeXeajIOvr+7fj6+56b07s9Qg==',	'administrator',	'[{"playerId":1}]');

DROP TABLE IF EXISTS "useraccesstoken";
CREATE TABLE "public"."useraccesstoken" (
    "token" character varying(100) NOT NULL,
    "expiresat" timestamp(0) NOT NULL,
    "user" character varying(100) NOT NULL,
    "playerid" integer NOT NULL,
    CONSTRAINT "useraccesstoken_pkey" PRIMARY KEY ("token")
) WITH (oids = false);

INSERT INTO "useraccesstoken" ("token", "expiresat", "user", "playerid") VALUES
('testtoken',	'2025-10-28 18:34:19',	'one',	1);

DROP TABLE IF EXISTS "world";
CREATE TABLE "public"."world" (
    "id" integer NOT NULL,
    "universe" character varying(200) NOT NULL,
    "timestamp" bigint NOT NULL,
    "name" character varying(100) NOT NULL,
    CONSTRAINT "world_pkey" PRIMARY KEY ("id")
) WITH (oids = false);

INSERT INTO "world" ("id", "universe", "timestamp", "name") VALUES
(2,	'Multiverse.SimpleUniverse.SimpleUniverse',	0,	'Some other world'),
(1,	'Multiverse.SimpleUniverse.SimpleUniverse',	2994,	'Test world');

ALTER TABLE ONLY "public"."message" ADD CONSTRAINT "fk_6fff93eb" FOREIGN KEY (world) REFERENCES world(id) NOT DEFERRABLE;
ALTER TABLE ONLY "public"."message" ADD CONSTRAINT "fk_e2bed869" FOREIGN KEY (player) REFERENCES player(id) NOT DEFERRABLE;

ALTER TABLE ONLY "public"."script" ADD CONSTRAINT "fk_ae3ca6e4" FOREIGN KEY (player) REFERENCES player(id) NOT DEFERRABLE;

ALTER TABLE ONLY "public"."unit" ADD CONSTRAINT "fk_50e2443b" FOREIGN KEY (world) REFERENCES world(id) NOT DEFERRABLE;
ALTER TABLE ONLY "public"."unit" ADD CONSTRAINT "fk_e3ea33d2" FOREIGN KEY (script) REFERENCES script(id) NOT DEFERRABLE;
ALTER TABLE ONLY "public"."unit" ADD CONSTRAINT "fk_fc18deb8" FOREIGN KEY (player) REFERENCES player(id) NOT DEFERRABLE;

ALTER TABLE ONLY "public"."unitgroup" ADD CONSTRAINT "fk_9884ebd3" FOREIGN KEY (world) REFERENCES world(id) NOT DEFERRABLE;
ALTER TABLE ONLY "public"."unitgroup" ADD CONSTRAINT "fk_a3a5e006" FOREIGN KEY (player) REFERENCES player(id) NOT DEFERRABLE;

ALTER TABLE ONLY "public"."unitgroupunits" ADD CONSTRAINT "fk_9d83f785" FOREIGN KEY (elt) REFERENCES unit(id) NOT DEFERRABLE;
ALTER TABLE ONLY "public"."unitgroupunits" ADD CONSTRAINT "fk_fe10774" FOREIGN KEY (unitgroup) REFERENCES unitgroup(id) NOT DEFERRABLE;

-- 2025-04-13 14:28:09.592141+00
