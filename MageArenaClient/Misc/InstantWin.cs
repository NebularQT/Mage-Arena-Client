using UnityEngine;
using Steamworks;

namespace MageArenaClient
{
    public static class InstantWin
    {
        public static void Trigger()
        {
            var mgr = GameObject.FindObjectOfType<PlayerRespawnManager>();
            if (mgr == null || mgr.pmv == null) return;

            int localTeam = mgr.pmv.playerTeam;
            int losingTeam = localTeam == 0 ? 1 : 0;

            mgr.SendMessage("EndGame", losingTeam);
        }
    }
}
    
