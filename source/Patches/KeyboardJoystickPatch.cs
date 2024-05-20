using HarmonyLib;
using TownOfUs.Patches;

namespace TownOfUs
{
    [HarmonyPatch(typeof(KeyboardJoystick), nameof(KeyboardJoystick.HandleHud))]
    public class KeyboardJoystickPatch
    {
        [HarmonyPostfix]
        public static void Postfix()
        {
            if (DestroyableSingleton<HudManager>.Instance != null && DestroyableSingleton<HudManager>.Instance.ImpostorVentButton != null && DestroyableSingleton<HudManager>.Instance.ImpostorVentButton.isActiveAndEnabled && ConsoleJoystick.player.GetButtonDown(50))
                DestroyableSingleton<HudManager>.Instance.ImpostorVentButton.DoClick();

            // Update to support position changes during mind control
            if (MindControlAbility.IsPlayerControlled(PlayerControl.LocalPlayer)) {
                // Prevent movement if the player is controlled
                return;
            }

            // Ensure compatibility with the new mind control ability
            // Prevent the controlled player's keyboard joystick (e.g., WASD) from working
            if (MindControlAbility.IsPlayerControlled(PlayerControl.LocalPlayer)) {
                DisablePlayerControl();
            }
            // Allow the controller player to move the controlled player
            else if (MindControlAbility.IsPlayerController(PlayerControl.LocalPlayer)) {
                EnableControllerControl();
            }
        }

        private static void DisablePlayerControl() {
            // Implementation to disable player's control
            PlayerControl.LocalPlayer.moveable = false;
        }

        private static void EnableControllerControl() {
            // Implementation to enable controller's control over another player
            var controlledPlayer = MindControlAbility.GetControlledPlayer(PlayerControl.LocalPlayer);
            if (controlledPlayer != null) {
                controlledPlayer.moveable = true;
            }
        }
    }
}
