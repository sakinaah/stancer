using System;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using MenuAPI;

namespace XStance
{
    public class XStanceMenu : BaseScript
    {
        private readonly Menu stanceMenu;
        private Vehicle currentVehicle;

        public XStanceMenu()
        {
            stanceMenu = new Menu("XStance", "Vehicle Stance Editor");

            var frontTrackWidth = new MenuSliderItem("Front Track Width", -25, 25, 0);
            var rearTrackWidth = new MenuSliderItem("Rear Track Width", -25, 25, 0);
            var frontCamber = new MenuSliderItem("Front Camber", -20, 20, 0);
            var rearCamber = new MenuSliderItem("Rear Camber", -20, 20, 0);

            stanceMenu.AddMenuItem(frontTrackWidth);
            stanceMenu.AddMenuItem(rearTrackWidth);
            stanceMenu.AddMenuItem(frontCamber);
            stanceMenu.AddMenuItem(rearCamber);

            var resetButton = new MenuItem("Reset All", "Reset all values to default.");
            stanceMenu.AddMenuItem(resetButton);

            stanceMenu.OnItemSelect += (menu, item, index) =>
            {
                if (item == resetButton && currentVehicle != null)
                {
                    ResetVehicleStance();
                    frontTrackWidth.Position = 0;
                    rearTrackWidth.Position = 0;
                    frontCamber.Position = 0;
                    rearCamber.Position = 0;
                }
            };

            stanceMenu.OnMenuClose += (menu) =>
            {
                if (currentVehicle != null)
                {
                    ApplyStanceChanges(frontTrackWidth.Position, rearTrackWidth.Position, frontCamber.Position, rearCamber.Position);
                }
            };

            API.RegisterCommand("stance", new Action(OpenMenu), false);
            API.RegisterKeyMapping("stance", "Open XStance Menu", "keyboard", "F6");

            Tick += MonitorPlayerVehicle;
        }

        private void OpenMenu()
        {
            stanceMenu.OpenMenu();
        }

        private async System.Threading.Tasks.Task MonitorPlayerVehicle()
        {
            await Delay(100);

            var playerPed = Game.PlayerPed;
            currentVehicle = playerPed.IsInVehicle() ? playerPed.CurrentVehicle : null;
        }

        private void ApplyStanceChanges(int frontTrack, int rearTrack, int frontCamber, int rearCamber)
        {
            if (currentVehicle == null) return;

            int wheelsCount = API.GetVehicleNumberOfWheels(currentVehicle.Handle);
            for (int i = 0; i < wheelsCount; i++)
            {
                float track = i < wheelsCount / 2 ? frontTrack / 10f : rearTrack / 10f;
                float camber = i < wheelsCount / 2 ? frontCamber / 10f : rearCamber / 10f;

                API.SetVehicleWheelXOffset(currentVehicle.Handle, i, track);
                API.SetVehicleWheelYRotation(currentVehicle.Handle, i, camber);
            }
        }

        private void ResetVehicleStance()
        {
            if (currentVehicle == null) return;

            int wheelsCount = API.GetVehicleNumberOfWheels(currentVehicle.Handle);
            for (int i = 0; i < wheelsCount; i++)
            {
                API.SetVehicleWheelXOffset(currentVehicle.Handle, i, 0);
                API.SetVehicleWheelYRotation(currentVehicle.Handle, i, 0);
            }
        }
    }
}