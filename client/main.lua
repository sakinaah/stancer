-- XStance Client Main Script
local currentVehicle = nil
local stanceData = {}

-- Monitor player vehicle state
Citizen.CreateThread(function()
    while true do
        Citizen.Wait(100)
        local ped = PlayerPedId()
        if IsPedInAnyVehicle(ped, false) then
            local vehicle = GetVehiclePedIsIn(ped, false)
            if GetPedInVehicleSeat(vehicle, -1) == ped then
                currentVehicle = vehicle
                stanceData[vehicle] = stanceData[vehicle] or {
                    frontTrackWidth = 0.0,
                    rearTrackWidth = 0.0,
                    frontCamber = 0.0,
                    rearCamber = 0.0
                }
            else
                currentVehicle = nil
            end
        else
            currentVehicle = nil
        end
    end
end)

-- Toggle menu
Citizen.CreateThread(function()
    while true do
        Citizen.Wait(0)
        if IsControlJustPressed(0, Config.ToggleMenuControl) and currentVehicle then
            ToggleMenu()
        end
    end
end)

function ToggleMenu()
    if Menu.isOpen then
        Menu:close()
    else
        Menu:open()
    end
end

-- Apply stance to vehicle
function ApplyStance(vehicle, data)
    if not DoesEntityExist(vehicle) then return end
    for i = 0, GetVehicleNumberOfWheels(vehicle) - 1 do
        SetVehicleWheelXOffset(vehicle, i, data.frontTrackWidth)
        SetVehicleWheelYRotation(vehicle, i, data.frontCamber)
    end
end

-- Commands
RegisterCommand("stance", function()
    ToggleMenu()
end, false)