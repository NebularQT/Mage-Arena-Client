using HarmonyLib;
using Steamworks;

namespace MageArenaClient;

[HarmonyPatch(typeof(MainMenuManager), "Start")]
public class PreventModdingStatPatch
{
    static void Prefix()
    {
        SteamUserStats.SetStat("modding", 0);
    }
    //idk if this works server side but Yeah hopefully it does
    static void Postfix()
    {
        SteamUserStats.SetStat("modding", 0);
    }
}