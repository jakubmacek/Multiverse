"use strict";
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
    abilities.use(self, 'Move', null, x, y);
    return null;
}
