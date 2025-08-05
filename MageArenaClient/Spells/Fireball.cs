using System.Collections;
using System.Reflection;
using FishNet.Object;
using HarmonyLib;
using UnityEngine;
namespace MageArenaClient;
[HarmonyPatch(typeof(PlayerInventory), "cFireball")]
public class FireballCast
{
    static bool Prefix(PlayerInventory __instance)
    {
        if (!ToggleStates.SkipCooldown) return true;

        var pm = __instance.GetComponent<PlayerMovement>();
        if (pm == null || pm.isDead || !__instance.initedInv)
            return false;

        // Reflect private fields
        int equippedIndex = (int)AccessTools.Field(typeof(PlayerInventory), "equippedIndex").GetValue(__instance);
        var equippedItems = (GameObject[])AccessTools.Field(typeof(PlayerInventory), "equippedItems").GetValue(__instance);

        if (equippedItems == null || equippedIndex < 0 || equippedIndex >= equippedItems.Length)
            return false;

        var equipped = equippedItems[equippedIndex];
        if (equipped == null || !equipped.TryGetComponent<MageBookController>(out var mageBook))
            return false;

        if (mageBook.LastPressedPage != 1)
            return false;

        mageBook.Fireball(__instance.gameObject, pm.level);
        mageBook.ReinstateFireballEmis();

        return false;
    }
}

[HarmonyPatch(typeof(MageBookController), nameof(MageBookController.Fireball))]
public class FireballBehavior
{
    static void Postfix(MageBookController __instance, GameObject ownerobj, int level)
    {
        __instance.StartCoroutine(HandleFireMode(__instance, ownerobj, level));
    }

    private static IEnumerator HandleFireMode(MageBookController instance, GameObject owner, int level)
    {
        var type = instance.GetType();
        var method = type.GetMethod("ShootFireballServer", BindingFlags.NonPublic | BindingFlags.Instance);
        if (method == null)
            yield break;

        Vector3 baseDir = Camera.main.transform.forward;
        Vector3 firePoint = instance.firePoint.position;

        switch (ToggleStates.CurrentFireMode)
        {
            case FireMode.Normal:
                method.Invoke(instance, new object[] { owner, baseDir, level, firePoint });
                break;

            case FireMode.Burst:
                for (int i = 0; i < 5; i++)
                {
                    Vector3 currentDir = Camera.main.transform.forward;
                    Vector3 currfirePoint = instance.firePoint.position;
                    method.Invoke(instance, new object[] { owner, currentDir, level, currfirePoint });
                    yield return new WaitForSeconds(0.2f);
                }
                break;
            case FireMode.Shotgun:
                int pelletCount = 5;
                float spreadAngle = 15f;

                for (int i = 0; i < pelletCount; i++)
                {
                    Vector3 spread = Quaternion.Euler(
                        Random.Range(-spreadAngle, spreadAngle),
                        Random.Range(-spreadAngle, spreadAngle),
                        0) * baseDir;

                    method.Invoke(instance, new object[] { owner, spread.normalized, level, firePoint });
                }
                break;
        }
    }
}
    

