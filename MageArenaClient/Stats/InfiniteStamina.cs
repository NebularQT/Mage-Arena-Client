using HarmonyLib;
using System.Reflection;
using UnityEngine;
using FishNet.Object;

namespace MageArenaClient
{
    public static class InfiniteStaminaM
    {
        [HarmonyPatch(typeof(PlayerMovement), "Update")]
        public class InfiniteStamina
        {
            static void Postfix(PlayerMovement __instance)
            {
                var netObj = __instance.GetComponent<NetworkObject>();
                if (netObj == null || !netObj.IsOwner)
                    return;

                if (!ToggleStates.InfiniteStamina)
                    return;

                var stam = typeof(PlayerMovement).GetField("stamina", BindingFlags.Instance | BindingFlags.NonPublic);

                if (stam != null && (float)stam.GetValue(__instance) < 10f)
                {
                    stam.SetValue(__instance, 10f);
                }
            }
        }
    }
}