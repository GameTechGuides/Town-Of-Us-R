using System;
using System.Linq;
using Hazel;
using TownOfUs.CrewmateRoles.MedicMod;
using TownOfUs.Extensions;
using TownOfUs.Patches;
using UnityEngine;

namespace TownOfUs.Roles
{
    public class Inquisitor : Role
    {
        public int InquireUses = int.MaxValue;
        public int VanquishUses = 3;
        public DateTime LastInquired;
        public DateTime LastVanquished;
        public bool VanquishOnCooldown = false;

        public Inquisitor(PlayerControl player) : base(player)
        {
            Name = "Inquisitor";
            ImpostorText = () => "Eliminate the heretics.";
            TaskText = () => "Use Inquire to learn if a player is a heretic.\nUse Vanquish to eliminate heretics.";
            Color = Patches.Colors.Inquisitor;
            RoleType = RoleEnum.Inquisitor;
            Faction = Faction.NeutralEvil;
        }

        public float InquireCooldown()
        {
            var utcNow = DateTime.UtcNow;
            var timeSpan = utcNow - LastInquired;
            var num = CustomGameOptions.InquireCooldown * 1000f;
            var flag2 = num - (float)timeSpan.TotalMilliseconds < 0f;
            if (flag2) return 0;
            return (num - (float)timeSpan.TotalMilliseconds) / 1000f;
        }

        public float VanquishCooldown()
        {
            var utcNow = DateTime.UtcNow;
            var timeSpan = utcNow - LastVanquished;
            var num = CustomGameOptions.VanquishCooldown * 1000f;
            var flag2 = num - (float)timeSpan.TotalMilliseconds < 0f;
            if (flag2) return 0;
            return (num - (float)timeSpan.TotalMilliseconds) / 1000f;
        }

        public bool IsHeretic(PlayerControl target)
        {
            // Logic to determine if the target is a heretic
            // This is a placeholder logic, replace it with the actual game logic
            return target.Data.IsImpostor || target.Is(RoleEnum.Jester) || target.Is(RoleEnum.Arsonist);
        }

        public void Inquire(PlayerControl target)
        {
            if (InquireUses <= 0) return;
            LastInquired = DateTime.UtcNow;
            InquireUses--;
            if (IsHeretic(target))
            {
                // Inform the Inquisitor that the target is a heretic
                // This is a placeholder, replace it with actual notification logic
                Utils.Rpc(CustomRPC.InformInquisitor, Player.PlayerId, target.PlayerId, true);
            }
            else
            {
                // Inform the Inquisitor that the target is not a heretic
                // This is a placeholder, replace it with actual notification logic
                Utils.Rpc(CustomRPC.InformInquisitor, Player.PlayerId, target.PlayerId, false);
            }
        }

        public void Vanquish(PlayerControl target)
        {
            if (VanquishUses <= 0 || VanquishOnCooldown) return;
            LastVanquished = DateTime.UtcNow;
            if (IsHeretic(target))
            {
                VanquishUses--;
                Utils.Rpc(CustomRPC.VanquishHeretic, Player.PlayerId, target.PlayerId);
            }
            else
            {
                VanquishUses = 0;
                VanquishOnCooldown = true;
                // Inform the Inquisitor that they have lost all uses of Vanquish
                // This is a placeholder, replace it with actual notification logic
                Utils.Rpc(CustomRPC.LoseAllVanquish, Player.PlayerId);
            }
        }
    }
}
