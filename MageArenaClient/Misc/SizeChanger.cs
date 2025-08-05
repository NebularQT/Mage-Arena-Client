using HarmonyLib;
using FishNet.Object;
using UnityEngine;

public static class SizeChanger
{
    public static PlayerMovement localPlayer;

    public static void SetLocalPlayer(PlayerMovement player)
    {
        localPlayer = player;
    }

    [ServerRpc]
    public static void RequestChangeSize(float newScale)
    {
        if (localPlayer == null) return;
        localPlayer.transform.localScale = new Vector3(newScale, newScale, newScale);
        ShrinkEveryoneRpc(newScale);
    }

    [ObserversRpc]
    public static void ShrinkEveryoneRpc(float newScale)
    {
        if (localPlayer == null) return;
        localPlayer.transform.localScale = new Vector3(newScale, newScale, newScale);
    }

    [HarmonyPatch(typeof(PlayerMovement), "Update")]
    public class SizeChangerPatch
    {
        static void Postfix(PlayerMovement __instance)
        {
            var netObj = __instance.GetComponent<NetworkObject>();
            if (netObj != null && netObj.IsOwner && localPlayer == null)
            {
                SetLocalPlayer(__instance);
            }
        }
    }
}
