using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using Newtonsoft.Json;

namespace XStance
{
    public class XStanceMain : BaseScript
    {
        private readonly Dictionary<int, VehicleStanceData> vehicleStances = new();
        private int currentVehicle;

        public XStanceMain()
        {
            Tick += MonitorPlayerVehicle;
            EventHandlers["xstance:applyStance"] += new Action<int, string>(ApplyStanceFromData);
            EventHandlers["xstance:getStance"] += new Action<int>(SendVehicleStance);

            Debug.WriteLine("[XStance] Initialized.");
        }

        private async Task MonitorPlayerVehicle()
        {
            await Delay(500);

            int playerPed = API.PlayerPedId();
            if (API.IsPedInAnyVehicle(playerPed, false))
            {
                int vehicle = API.GetVehiclePedIsIn(playerPed, false);
                if (API.GetPedInVehicleSeat(vehicle, -1) == playerPed)
                {
                    currentVehicle = vehicle;
                    if (!vehicleStances.ContainsKey(vehicle))
                    {
                        vehicleStances[vehicle] = new VehicleStanceData();
                    }
                    ApplyStanceToVehicle(vehicle, vehicleStances[vehicle]);
                }
            }
            else
            {
                currentVehicle = 0;
            }
        }

        private void ApplyStanceFromData(int vehicle, string jsonData)
        {
            try
            {
                var stanceData = JsonConvert.DeserializeObject<VehicleStanceData>(jsonData);
                vehicleStances[vehicle] = stanceData;
                ApplyStanceToVehicle(vehicle, stanceData);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[XStance] Error applying stance: {ex.Message}");
            }
        }

        private void SendVehicleStance(int vehicle)
        {
            if (vehicleStances.TryGetValue(vehicle, out var stanceData))
            {
                string json = JsonConvert.SerializeObject(stanceData);
                TriggerEvent("xstance:stanceData", vehicle, json);
            }
        }

        private void ApplyStanceToVehicle(int vehicle, VehicleStanceData stanceData)
        {
            if (!API.DoesEntityExist(vehicle)) return;

            int wheelsCount = API.GetVehicleNumberOfWheels(vehicle);
            for (int i = 0; i < wheelsCount; i++)
            {
                float offset = i < wheelsCount / 2 ? stanceData.FrontTrackWidth : stanceData.RearTrackWidth;
                float camber = i < wheelsCount / 2 ? stanceData.FrontCamber : stanceData.RearCamber;

                API.SetVehicleWheelXOffset(vehicle, i, offset);
                API.SetVehicleWheelYRotation(vehicle, i, camber);
            }

            UpdateVehicleDecorators(vehicle, stanceData);
        }

        private void UpdateVehicleDecorators(int vehicle, VehicleStanceData stanceData)
        {
            API.DecorSetFloat(vehicle, "xstance_front_track", stanceData.FrontTrackWidth);
            API.DecorSetFloat(vehicle, "xstance_rear_track", stanceData.RearTrackWidth);
            API.DecorSetFloat(vehicle, "xstance_front_camber", stanceData.FrontCamber);
            API.DecorSetFloat(vehicle, "xstance_rear_camber", stanceData.RearCamber);
        }
    }

    public class VehicleStanceData
    {
        public float FrontTrackWidth { get; set; } = 0.0f;
        public float RearTrackWidth { get; set; } = 0.0f;
        public float FrontCamber { get; set; } = 0.0f;
        public float RearCamber { get; set; } = 0.0f;
    }
}