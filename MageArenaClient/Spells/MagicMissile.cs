using System.Collections;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MageArenaClient;

//ADD LOGIC FOR OTHER TYPES OF THIS SPELL



[HarmonyPatch(typeof(MageBookController), "CastWard")]
public class CastWardM
{
    static void Postfix(MageBookController __instance, GameObject ownerobj, int level)
    {
        GameObject target = null;
        //mimics game original targeting mechanics
        Collider[] colliderArray = Physics.OverlapSphere(__instance.transform.position, 60f, (int)__instance.playerlayer);
        float bestScore = float.MaxValue;

        foreach (Collider collider in colliderArray)
        {
            Vector3 toTarget = collider.transform.position - __instance.transform.position;
            float distance = toTarget.magnitude;
            float angle = Vector3.Angle(Camera.main.transform.forward, toTarget.normalized);

            if (angle > 90f) continue;

            if (collider.TryGetComponent<PlayerMovement>(out var playerMov))
            {
                if (playerMov.playerTeam != ownerobj.GetComponent<PlayerMovement>().playerTeam)
                {
                    float score = distance + angle * 0.5f;
                    if (score < bestScore)
                    {
                        bestScore = score;
                        target = playerMov.gameObject;
                    }
                }
            }
            else if (collider.TryGetComponent<GetPlayerGameobject>(out var getPlayer))
            {
                var pm = getPlayer.player.GetComponent<PlayerMovement>();
                if (pm != null && pm.playerTeam != ownerobj.GetComponent<PlayerMovement>().playerTeam)
                {
                    float score = distance + angle * 0.5f;
                    if (score < bestScore)
                    {
                        bestScore = score;
                        target = getPlayer.player;
                    }
                }
            }
            else if (collider.TryGetComponent<GetShadowWizardController>(out var shadow))
            {
                float score = distance + angle * 0.5f;
                if (score < bestScore)
                {
                    bestScore = score;
                    target = shadow.ShadowWizardAI;
                }
            }
        }

        MethodInfo shootMethod = AccessTools.Method(typeof(MageBookController), "ShootMagicMissleServer");

        GameObject runner = new GameObject("SpellRepeater");
        GameObject.DontDestroyOnLoad(runner);
        var repeater = runner.AddComponent<SpellRepeater>();

         //pew pew
        repeater.StartShootRoutine(__instance, ownerobj, target, level, shootMethod);
    }
}


[HarmonyPatch(typeof(PlayerInventory), "cCastWard")]
public class WardCast
{
    static bool Prefix(PlayerInventory __instance)
    {
        if (!ToggleStates.SkipCooldown) return true;

        var pm = __instance.GetComponent<PlayerMovement>();
        if (pm == null || pm.isDead || !__instance.initedInv)
            return false;

        int equippedIndex = (int)AccessTools.Field(typeof(PlayerInventory), "equippedIndex").GetValue(__instance);
        var equippedItems = (GameObject[])AccessTools.Field(typeof(PlayerInventory), "equippedItems").GetValue(__instance);

        if (equippedItems == null || equippedIndex < 0 || equippedIndex >= equippedItems.Length)
            return false;

        GameObject equipped = equippedItems[equippedIndex];
        if (equipped == null) return false;

        if (!equipped.TryGetComponent<MageBookController>(out var mageBook)) return false;
        if (mageBook.LastPressedPage != 4) return false;

        mageBook.CastWard(__instance.gameObject, pm.level);
        mageBook.ReinstateWardEmis();


        return false;
    }
}