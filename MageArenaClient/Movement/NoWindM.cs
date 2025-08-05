using HarmonyLib;
namespace MageArenaClient
{
    public static class NoWindM
    {
        [HarmonyPatch(typeof(PlayerMovement), "MountainWind")]
        public class NoWind
        {
            static bool Prefix()
            {
                return !ToggleStates.DisableWind;
            }
        }
    }
}
