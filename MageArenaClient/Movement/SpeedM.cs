using HarmonyLib;
using UnityEngine;
namespace MageArenaClient
{

    public static class SpeedM
    {
        [HarmonyPatch(typeof(PlayerMovement), "Update")]
        public class Speed
        {
            static void Postfix(PlayerMovement __instance)
            {
                if (!ToggleStates.SpeedEnabled)
                    return;

                if (__instance.canMove)
                {
                    Vector3 velocity = __instance.velocity;
                    float multiplier = 1.01f;
                    velocity.x *= multiplier;
                    velocity.z *= multiplier;
                    __instance.velocity = velocity;
                }
            }
        }
    }
}