using FishNet.Object;
using HarmonyLib;
using UnityEngine;

namespace MageArenaClient
{
    public static class InputHandler
    {
        public static bool UpdatedText = false;

        // Track if NoFall was enabled *before* NoClip was turned on
        private static bool noFallPriorToNoClip = false;

        public static void HandleInput()
        {
            if (Input.GetKeyDown(KeyCode.F11)) Toggle(ToggleStates.ESPEnabled = !ToggleStates.ESPEnabled);
            if (Input.GetKeyDown(KeyCode.Semicolon)) Toggle(ToggleStates.InputEnabled = !ToggleStates.InputEnabled);
            if (Input.GetKeyDown(KeyCode.Semicolon)) Toggle(ToggleStates.MenuEnabled = !ToggleStates.MenuEnabled);
            if (!ToggleStates.InputEnabled) return;


            if (Input.GetKeyDown(KeyCode.F1)) Toggle(ToggleStates.SpeedEnabled = !ToggleStates.SpeedEnabled);
            if (Input.GetKeyDown(KeyCode.F2)) Toggle(ToggleStates.InfiniteHealth = !ToggleStates.InfiniteHealth);
            if (Input.GetKeyDown(KeyCode.F3)) Toggle(ToggleStates.InfiniteStamina = !ToggleStates.InfiniteStamina);
            if (Input.GetKeyDown(KeyCode.F4)) Toggle(ToggleStates.DisableWind = !ToggleStates.DisableWind);
            if (Input.GetKeyDown(KeyCode.F5)) Toggle(ToggleStates.InfiniteJump = !ToggleStates.InfiniteJump);
            if (Input.GetKeyDown(KeyCode.F6)) Toggle(ToggleStates.NoFallEnable = !ToggleStates.NoFallEnable);
            if (Input.GetKeyDown(KeyCode.P)) Toggle(ToggleStates.SkipCooldown = !ToggleStates.SkipCooldown);
            if (Input.GetKeyDown(KeyCode.F7))Toggle(ToggleStates.FastPortcullisOpen = !ToggleStates.FastPortcullisOpen);
            
            //NO CLIP NO FALL CHECKER------------------------------------------------------------------------
            if (Input.GetKeyDown(KeyCode.N))
            {
                
                bool newNoClipState = !ToggleStates.NoClipEnabled;

                if (newNoClipState)
                {
                    noFallPriorToNoClip = ToggleStates.NoFallEnable;
                    ToggleStates.NoFallEnable = true; 
                }
                else
                {
                    if (!noFallPriorToNoClip)
                        ToggleStates.NoFallEnable = false;
                }

                ToggleStates.NoClipEnabled = newNoClipState;
                UpdatedText = true;
            }
            //-----------------------------------------------------------------------------------------------
            if (Input.GetKeyDown(KeyCode.L))
            {
                ToggleStates.CurrentFireMode = (FireMode)(((int)ToggleStates.CurrentFireMode + 1) % 3);
                UpdatedText = true;
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                ToggleStates.CurrentWardMode = (WardMode)(((int)ToggleStates.CurrentWardMode + 1) % 5);
                UpdatedText = true;
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                ToggleStates.CurrentFrostMode = (FrostMode)(((int)ToggleStates.CurrentFrostMode + 1) % 3);
                UpdatedText = true;
            }
            if (Input.GetKeyDown(KeyCode.F10))
            {
                ToggleStates.CurrentHealingAmount = (HealingAmount)(((int)ToggleStates.CurrentHealingAmount + 1) % 4);
                UpdatedText = true;
            }


            if (Input.GetKeyDown(KeyCode.Equals) || Input.GetKeyDown(KeyCode.KeypadPlus))
            {
                ToggleStates.PlayerScale = Mathf.Min(ToggleStates.PlayerScale + ToggleStates.ScaleStep, ToggleStates.MaxScale);
                SizeChanger.RequestChangeSize(ToggleStates.PlayerScale);
                UpdatedText = true;
            }

            if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus))
            {
                ToggleStates.PlayerScale = Mathf.Max(ToggleStates.PlayerScale - ToggleStates.ScaleStep, ToggleStates.MinScale);
                SizeChanger.RequestChangeSize(ToggleStates.PlayerScale);
                UpdatedText = true;
            }

            if (Input.GetKeyDown(KeyCode.F8)) KillAll.KillEveryone();
            if (Input.GetKeyDown(KeyCode.J)) KillNearest.Execute();
            if (Input.GetKeyDown(KeyCode.F9)) InstantWin.Trigger();
        }

        private static void Toggle(bool _) => UpdatedText = true;
    }
}