using HarmonyLib;
using System.Reflection;
using UnityEngine;
namespace MageArenaClient
{
    public static class InfiniteHealthModule
    {
        [HarmonyPatch(typeof(PlayerMovement), "Update")]
        public class InfiniteHealthPatch
        {
            static readonly FieldInfo healthField = typeof(PlayerMovement).GetField("playerHealth", BindingFlags.Instance | BindingFlags.NonPublic);

            static void Postfix(PlayerMovement __instance)
            {
                if (!ToggleStates.InputEnabled) return;

                if (Input.GetKey(KeyCode.H))
                {
                    if ((float)healthField.GetValue(__instance) <= 100f)
                        healthField.SetValue(__instance, 100f);
                }

                if (!ToggleStates.InfiniteHealth || healthField == null)
                    return;

                if (healthField.GetValue(__instance) is not float currentHealth)
                    return;

                float targetHealth = 100f;

                switch (ToggleStates.CurrentHealingAmount)
                {
                    case HealingAmount.Normal:
                        targetHealth = 100f;
                        if (currentHealth < targetHealth || currentHealth > 150)
                            healthField.SetValue(__instance, targetHealth);
                        break;
                    case HealingAmount.NormalPlus:
                        targetHealth = 106f;
                        if (currentHealth < targetHealth || currentHealth > 150)
                            healthField.SetValue(__instance, targetHealth);
                        break;
                    case HealingAmount.Stew:
                        targetHealth = 150f;
                        if(currentHealth < targetHealth || currentHealth > 150)
                                healthField.SetValue(__instance, targetHealth);
                        break;
                    case HealingAmount.HackerProof:
                        targetHealth = 99999f;
                        if (currentHealth < targetHealth)
                        {
                            healthField.SetValue(__instance, targetHealth);
                        }
                        break;
                }
            }
        }
    }
}
