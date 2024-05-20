using System.Collections.Generic;
using UnityEngine;

namespace TownOfUs.Patches
{
    public class MindControlAbility
    {
        private static Dictionary<PlayerControl, PlayerControl> controlledPlayers = new Dictionary<PlayerControl, PlayerControl>();

        public static void ControlPlayer(PlayerControl controller, PlayerControl target)
        {
            if (controller == null || target == null) return;
            controlledPlayers[controller] = target;
        }

        public static bool IsPlayerControlled(PlayerControl player)
        {
            return controlledPlayers.ContainsValue(player);
        }

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
