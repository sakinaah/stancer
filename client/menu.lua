local Menu = {}
Menu.items = {
    { label = "Front Track Width", value = 0, min = -25, max = 25 },
    { label = "Rear Track Width", value = 0, min = -25, max = 25 },
    { label = "Front Camber", value = 0, min = -20, max = 20 },
    { label = "Rear Camber", value = 0, min = -20, max = 20 },
}
Menu.selectedIndex = 1
Menu.isOpen = false

function Menu:open()
    self.isOpen = true
    SetNuiFocusKeepInput(true)
end

function Menu:close()
    self.isOpen = false
    SetNuiFocusKeepInput(false)
end

function Menu:draw()
    if not self.isOpen then return end

    local x, y = 0.8, 0.2
    for i, item in ipairs(self.items) do
        local color = { r = 255, g = 255, b = 255 } -- Default color
        if i == self.selectedIndex then
            color = { r = 255, g = 255, b = 0 } -- Highlight selected item
        end
        DrawText(x, y + (i * 0.03), item.label .. ": " .. item.value, color)
    end
end

function Menu:handleInput()
    if not self.isOpen then return end

    if IsControlJustPressed(0, 172) then -- Arrow Up
        self.selectedIndex = self.selectedIndex > 1 and self.selectedIndex - 1 or #self.items
    elseif IsControlJustPressed(0, 173) then -- Arrow Down
        self.selectedIndex = self.selectedIndex < #self.items and self.selectedIndex + 1 or 1
    elseif IsControlJustPressed(0, 174) then -- Arrow Left
        local item = self.items[self.selectedIndex]
        item.value = math.max(item.min, item.value - 1)
    elseif IsControlJustPressed(0, 175) then -- Arrow Right
        local item = self.items[self.selectedIndex]
        item.value = math.min(item.max, item.value + 1)
    end
end

function DrawText(x, y, text, color)
    SetTextFont(0)
    SetTextProportional(1)
    SetTextScale(0.35, 0.35)
    SetTextColour(color.r, color.g, color.b, 255)
    SetTextOutline()
    SetTextEntry("STRING")
    AddTextComponentString(text)
    DrawText(x, y)
end

Citizen.CreateThread(function()
    while true do
        Citizen.Wait(0)
        Menu:draw()
        Menu:handleInput()
    end
end)

return Menu