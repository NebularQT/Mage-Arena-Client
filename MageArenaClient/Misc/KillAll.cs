using UnityEngine;

namespace MageArenaClient
{
    public static class KillAll
    {
        public static void KillEveryone()
        {
            try
            {
                var respawnMG = GameObject.FindObjectOfType<PlayerRespawnManager>();
                if (respawnMG?.pmv == null) return;

                int myTeam = respawnMG.pmv.playerTeam;
                foreach (var pm in Object.FindObjectsOfType<PlayerMovement>())
                {
                    if (pm == null || pm.isDead || pm.playerTeam == myTeam) continue;

                    var oneShot = typeof(PlayerMovement).GetMethod("ExcaliburDamagePlayer",
                        System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
                    oneShot?.Invoke(pm, new object[] { 105f, null });
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
            }
        }
    }
}
