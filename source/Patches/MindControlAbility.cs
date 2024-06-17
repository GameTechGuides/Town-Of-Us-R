using System.Collections.Generic;
using UnityEngine;

namespace TownOfUs.Patches
{
    // This class handles the logic for the Mind Control ability in the game.
    public class MindControlAbility
    {
        // Dictionary to keep track of which players are controlling which other players.
        private static Dictionary<PlayerControl, PlayerControl> controlledPlayers = new Dictionary<PlayerControl, PlayerControl>();

        // Method to apply the mind control effect from a controller to a target player.
        public static void ControlPlayer(PlayerControl controller, PlayerControl target)
        {
            if (controller == null || target == null) return;
            controlledPlayers[controller] = target;
        }

        // Method to check if a player is currently being controlled by another player.
        public static bool IsPlayerControlled(PlayerControl player)
        {
            return controlledPlayers.ContainsValue(player);
        }

        // Method to transfer control of a player from one controller to another.
        public static void TransferControl(PlayerControl newController, PlayerControl target)
        {
            if (newController == null || target == null) return;
            var currentController = GetControllerOfPlayer(target);
            if (currentController != null)
            {
                controlledPlayers.Remove(currentController);
            }
            controlledPlayers[newController] = target;
        }

        // Method to get the controller of a player who is being controlled.
        public static PlayerControl GetControllerOfPlayer(PlayerControl player)
        {
            foreach (var kvp in controlledPlayers)
            {
                if (kvp.Value == player)
                {
                    return kvp.Key;
                }
            }
            return null;
        }
    }
}
