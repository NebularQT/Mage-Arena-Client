using HarmonyLib;
using UnityEngine;
using System.Reflection;
namespace MageArenaClient
{
    public static class InfiniteJumpM
    {
        [HarmonyPatch(typeof(PlayerMovement), "Update")]
        public class InfiniteJump
        {
            static void Postfix(PlayerMovement __instance)
            {
                if (!ToggleStates.InfiniteJump)
                    return;

                __instance.canJump = true;

                var staminaF = typeof(PlayerMovement).GetField("stamina", BindingFlags.Instance | BindingFlags.NonPublic);
                if (staminaF != null)
                    staminaF.SetValue(__instance, 10f);

                var controllerF = typeof(PlayerMovement).GetField("characterController", BindingFlags.Instance | BindingFlags.NonPublic);
                var velocityF = typeof(PlayerMovement).GetField("velocity", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                var jumpSpeedF = typeof(PlayerMovement).GetField("jumpSpeed", BindingFlags.Instance | BindingFlags.NonPublic);

                if (controllerF == null || velocityF == null || jumpSpeedF == null)
                    return;

                var cc = controllerF.GetValue(__instance) as CharacterController;
                var velocity = (Vector3)velocityF.GetValue(__instance);
                float jumpSpeed = (float)jumpSpeedF.GetValue(__instance);

                if (cc == null)
                    return;

                bool canJump = cc.isGrounded && Mathf.Abs(velocity.y) < 0.1f;
                bool jumpPressed = Input.GetKeyDown(KeyCode.Space);

                if (jumpPressed || canJump)
                {
                    velocity.y = jumpSpeed * 1.5f;
                    velocityF.SetValue(__instance, velocity);
                }
            }
        }
    }
}