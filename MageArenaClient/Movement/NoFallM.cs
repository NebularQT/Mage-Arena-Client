using HarmonyLib;
namespace MageArenaClient
{
    public static class NoFallM
    {
        [HarmonyPatch(typeof(PlayerMovement), "UpdateMovement")]
        public class NoFall
        {
            static void Prefix(PlayerMovement __instance)
            {
                if (!ToggleStates.NoFallEnable) return;

                var blockFallDmgField = AccessTools.Field(typeof(PlayerMovement), "BlockFallDmgFrames");
                blockFallDmgField.SetValue(__instance, 1);
            }
        }
    }
}