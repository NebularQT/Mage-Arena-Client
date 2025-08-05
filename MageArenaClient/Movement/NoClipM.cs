using HarmonyLib;
using UnityEngine;

namespace MageArenaClient
{
    public static class NoClipM
    {
        [HarmonyPatch(typeof(PlayerMovement), "Update")]
        public class NoClip
        {
            private static bool noclipEnabled = false;
            private static float noclipSpeed = 45f;

            static void Postfix(PlayerMovement __instance)
            {
                if (!ToggleStates.InputEnabled) return;

                if (Input.GetKeyDown(KeyCode.N))
                {
                    noclipEnabled = !noclipEnabled;

                    var cc = __instance.GetComponent<CharacterController>();
                    if (cc != null)
                        cc.enabled = !noclipEnabled;
                }

                if (!noclipEnabled) return;

                Camera cam = Camera.main;
                if (cam == null) return;

                Vector3 forward = cam.transform.forward;
                Vector3 right = cam.transform.right;
                Vector3 up = Vector3.up;

                forward.y = 0;
                right.y = 0;

                forward.Normalize();
                right.Normalize();

                Vector3 move = Vector3.zero;

                move += forward * Input.GetAxisRaw("Vertical");
                move += right * Input.GetAxisRaw("Horizontal");

                if (Input.GetKey(KeyCode.Space)) move += up;
                if (Input.GetKey(KeyCode.LeftControl)) move -= up;

                move = move.normalized * noclipSpeed * Time.deltaTime;

                __instance.transform.position += move;
            }
        }
    }
}