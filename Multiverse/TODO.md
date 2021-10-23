* Možné skriptovací jazyky: Lua, TypeScript (JavaScript)


* Unit budou mit nejake schopnosti
* Places budou v hexa mrizce
* Places a group jsou jedine, co muzou mit skripty. Skripty budou delat handlery.
* Muzou mit tim i bitvy.
* Budovy budou specialni unit, ktere se nepohybuji.
* Place i group musi mit vlastnika, ten urcuje prirazeny skript.
* Souradnice muzou jit i do zapornych cisel.
* Je nutne poresit persistenci sveta. Neco jako ma Unity. Serializovani objektu.
* Unit muzou mit schopnosti. Ty muzou spotrebovavat resource a budou mit cooldown. Vymyslet jak udelat 3 pouziti v ramci jednoho cooldown.
* Unit a places muzou komunikovat. Skript prideleny k nejakemu place muze delat generala a rozdavat rozkazy. Pri prijeti rozkazu se probudi handler dane unit. Resit nejak omezeny dosah viditelnosti?
* Musi byt nejake resources. Umistene v place nebo unit. Takze place i unit musi mit omezenou kapacitu resource. Mozna to dat jen do unit a uvnitr place budou resources v ramci buildings.
* Pohyb bude taky nejaka akce, kterou ale budou muset vsechny unit v group vykonavat spolecne? Nebo budou moci unit v ramci group byt kazda jinde?
* Tezba surovin bude akce unit. Ta tim ziska do sve kapacity resource a musi ho predat jine unit - skladu.
* Zdroje surovin budou neznicitelne unit predgenerovane v places.
* Bude potreba nejaka fronta komunikace a spousteni skriptu. Neni dobre, aby vsichni reagovali ihned. Zahltilo by to vykon a pamet.
* Asi zacit nejakym webovym rozhranim v react nebo vue, kterym se to bude ovladat rucne. A k tomu API.
* Uzivatele a autentifikace. Teoreticky by jeden user mohl mit vice account/player a opacne jeden player byt ovladan vice uzivateli.
* Pouzit nejaky princip ticks nebo realny cas? Pripadne realny cas, ale nejake fiktivni datum?
* Unit by mely mit nejake zdravi. To by mohl byt specialni resource?
* Mapu kreslit pomoci CSS nebo rovnou pomoci SVG?
* Unit nevidi uplne vsechno na cele mape, ale jen v ramci sveho dosahu. Vymyslet, jak si budou moct jednotky ukladat a predavat nejake informace. Treba souradnice place, kam maji chodit.
* Pathfinding bude nejaka standardni knihovna.
* Budovani budov bude akce nejake worker unit a bude tam unit construction site, ktery bude sbirat resource "building work" nebo tak neco. Az je bude mit, tak se pretransformuje na cilovou budovu. Construction site by se mel uklidit z databaze ihned po vytvoreni budovy.
* Unit muzou byt taky "dead" nebo "destroyed". V takovem pripade nemuzou delat zadnou akci ani spoustet skripty, ale zustavaji soucasti skupiny a umistene na danem place. Takze je potrebuju uklidit po nejakem case.
* Omezit smer pohybu resources. Napriklad ConstructionSite bude mit kapacitu maximalne dle budovane budovy a nebude mozne z nej material odebirat.
