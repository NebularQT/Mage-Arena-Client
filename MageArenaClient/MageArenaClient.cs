using System.Reflection;
using BepInEx;
using FishNet.Managing.Timing;
using HarmonyLib;
using Steamworks;
using UnityEngine;

namespace MageArenaClient
{
    [BepInPlugin("helloguys", "MageArenaClient", "1.0.0")]
    public class MageArenaClient : BaseUnityPlugin
    {
        private float updateTime = 0f;
        private const float refreshSpeed = 0.5f;

        private void Awake()
        {
            Logger.LogInfo("MageArenaClient has been loaded");
            gameObject.AddComponent<StatusHUDRenderer>();
            gameObject.AddComponent<SimpleESP>();
            new Harmony("helloguys").PatchAll();
        }

        private void Update()
        {
            InputHandler.HandleInput();
            updateTime += Time.deltaTime;

            if (updateTime >= refreshSpeed || InputHandler.UpdatedText)
            {
                updateTime = 0f;
                //StatusDisplay.Update();
                InputHandler.UpdatedText = false;
            }
        }
    }
    public class StatusHUDRenderer : MonoBehaviour
    {
        void OnGUI()
        {
            StatusDisplay.OnGUI();
        }
    }
}
