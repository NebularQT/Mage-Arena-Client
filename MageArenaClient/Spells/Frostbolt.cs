using System.Collections;
using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace MageArenaClient;

[HarmonyPatch(typeof(MageBookController), nameof(MageBookController.Frostbolt))]
public class FrostboltM
{
    static void Postfix(MageBookController __instance, GameObject ownerobj, int level)
    {
        switch (ToggleStates.CurrentFrostMode)
        {
            case FrostMode.Normal:
                break;

            case FrostMode.Railgun:
                __instance.StartCoroutine(SpamFrostbolts(__instance, ownerobj, level, 10, 0.1f));
                break;

            case FrostMode.Wall:
                __instance.StartCoroutine(FrostWall(__instance, ownerobj, level, 1, 0.05f));
                break;
        }
    }

    private static IEnumerator SpamFrostbolts(MageBookController instance, GameObject owner, int level, int count, float delay)
    {
        var method = instance.GetType().GetMethod("ShootFrostboltServer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (method == null)
        {
            yield break;
        }


        Vector3 forward = Camera.main.transform.forward;
        for (int i = 0; i < count; i++)
        {
            forward = Camera.main.transform.forward;

            method.Invoke(instance, new object[] { owner, forward, level });
            yield return new WaitForSeconds(delay);
        }
    }
    private static IEnumerator FrostWall(MageBookController instance, GameObject owner, int level, int waveCount, float delay)
    {
        var method = instance.GetType().GetMethod("ShootFrostboltServer", BindingFlags.NonPublic | BindingFlags.Instance);
        if (method == null)
            yield break;

        Transform cam = Camera.main.transform;

        int width = 8;
        int height = 4;
        float spreadX = 10f;
        float spreadY = 6f;  

        Vector3 forward = cam.forward.normalized;
        Vector3 right = cam.right.normalized;
        Vector3 up = cam.up.normalized;

        Vector3 wallCenter = cam.position + forward * 10f; //move ahead so it doesnt mess with camera

        for (int wave = 0; wave < waveCount; wave++)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    float percentX = x / (float)(width - 1) - 0.5f;
                    float percentY = y / (float)(height - 1) - 0.5f;

                    Vector3 offset = (right * percentX * spreadX) + (up * percentY * spreadY);
                    Vector3 spawnPos = wallCenter + offset;
                    Vector3 direction = (spawnPos - cam.position).normalized;

                    method.Invoke(instance, new object[] { owner, direction, level });
                }
            }

            yield return new WaitForSeconds(delay);
        }
    }


    [HarmonyPatch(typeof(PlayerInventory), "cFrostbolt")]
    public class Frostbolt
    {
        static bool Prefix(PlayerInventory __instance)
        {
            if (!ToggleStates.SkipCooldown) return true;

            var pm = __instance.GetComponent<PlayerMovement>();
            if (pm == null || pm.isDead || !__instance.initedInv)
                return false;

            int equipIndex = (int)AccessTools.Field(typeof(PlayerInventory), "equippedIndex").GetValue(__instance);
            var Items = (GameObject[])AccessTools.Field(typeof(PlayerInventory), "equippedItems").GetValue(__instance);

            if (Items == null || equipIndex < 0 || equipIndex >= Items.Length)
                return false;

            GameObject equipped = Items[equipIndex];
            if (equipped == null) return false;

            if (!equipped.TryGetComponent<MageBookController>(out var MGbook)) return false;
            if (MGbook.LastPressedPage != 2) return false;


            MGbook.Frostbolt(__instance.gameObject, pm.level);

            MGbook.ReinstateFrostBoltEmis();

            return false;
        }
    }
}