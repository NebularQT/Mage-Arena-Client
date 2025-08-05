using HarmonyLib;
using UnityEngine;
using FishNet.Object;

namespace MageArenaClient
{
    [HarmonyPatch(typeof(PortcullisController), nameof(PortcullisController.GetInteractTimer))]
    public static class FastPortcullisOpen
    {
        static void Postfix(GameObject player, ref float __result)
        {

            if (!ToggleStates.FastPortcullisOpen)
            {
                return;
            }

            var netObj = player.GetComponent<NetworkObject>();
            if (netObj != null && netObj.IsOwner)
            {
                __result = 0.75f;
            }
        }
    }
}