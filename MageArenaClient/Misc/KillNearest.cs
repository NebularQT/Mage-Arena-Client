using UnityEngine;
using FishNet.Object;

namespace MageArenaClient
{
    public static class KillNearest
    {
        public static void Execute()
        {
            try
            {
                PlayerMovement local = null;
                foreach (var pm in Object.FindObjectsOfType<PlayerMovement>()) //laggy
                {
                    if (pm.GetComponent<NetworkObject>()?.IsOwner == true)
                    {
                        local = pm;
                        break;
                    }
                }
                if (local == null) return;

                PlayerMovement closest = null;
                float closestDist = float.MaxValue;

                foreach (var pm in Object.FindObjectsOfType<PlayerMovement>()) // laggy
                {
                    if (pm == null || pm == local || pm.isDead) continue;

                    float dist = Vector3.Distance(local.transform.position, pm.transform.position);
                    if (dist < closestDist)
                    {
                        closest = pm;
                        closestDist = dist;
                    }
                }

                var method = typeof(PlayerMovement).GetMethod("ExcaliburDamagePlayer",
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
                method?.Invoke(closest, new object[] { 105f, null });
            }
            catch (System.Exception e)
            {
                Debug.LogError($"KillNearest Error: {e}");
            }
        }
    }
}
