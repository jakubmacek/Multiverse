function list_to_table(list)
    local table = {};
    local i
    for i = 0, list.Count - 1 do
        table[i + 1] = list[i]
    end
    return table
end

function filter_by_type(type, table)
    local i = 0
    local newtable = {}
    for _, unit in ipairs(table) do
        if unit.type == type then
            i = i + 1
            newtable[i] = unit
        end
    end
    return newtable
end

function first(table)
    if type(table) ~= 'table' then
        table = list_to_table(table)
    end

    for _, item in ipairs(table) do
        return item
    end
    return nil
end

function onevent(self, event)
    local forests = filter_by_type(Forest, list_to_table(scanning.scanAround(self)))
    local forest = first(forests)
    if forest then
        if forest.x == self.x and forest.y == self.y then
            local harvestresult = abilities.use(self, 'HarvestWood', forest)
            print('harvest wood: ' .. harvestresult.type)
        else
            local movementability = first(self.abilities:withType(AbilityTypeMovement))
            local moveresult = abilities.use(self, movementability.name, nil, forest.x, forest.y)
            print('move to forest: ' .. moveresult.type)
        end
    end
end
