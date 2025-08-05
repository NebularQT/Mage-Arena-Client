using UnityEngine;

namespace MageArenaClient
{
    public static class StatusDisplay
    {
        private static bool visible = true;
        private static GUIStyle labelStyle;
        private static GUIStyle keyStyle;
        private static Rect panelRect;
        private static readonly Vector2 offset = new Vector2(-320, 20);

        
        public static void OnGUI()
        {
            if (!ToggleStates.MenuEnabled) return;

            InitStyles();

            float width = 300;
            float height = 515;
            panelRect = new Rect(Screen.width + offset.x, offset.y, width, height);

            GUI.color = new Color(0f, 0f, 0f, 0.6f);
            GUI.Box(panelRect, GUIContent.none);
            GUI.color = Color.white;

            GUILayout.BeginArea(panelRect);
            GUILayout.Space(6);

            DrawStatus("F1", "Speed", ToggleStates.SpeedEnabled);
            DrawStatus("F2", "Infinite Health", ToggleStates.InfiniteHealth);
            DrawStatus("F10", "Health/f", ToggleStates.CurrentHealingAmount.ToString());
            DrawStatus("F3", "Infinite Stamina", ToggleStates.InfiniteStamina);
            DrawStatus("F4", "No Wind", ToggleStates.DisableWind);
            DrawStatus("F5", "Infinite Jump", ToggleStates.InfiniteJump);
            DrawStatus("F6", "No Fall", ToggleStates.NoFallEnable);
            DrawStatus("F7", "Fast Gate", ToggleStates.FastPortcullisOpen);
            DrawStatus("N", "No Clip", ToggleStates.NoClipEnabled);
            DrawStatus("P", "No CD", ToggleStates.SkipCooldown);
            DrawStatus("F11", "ESP", ToggleStates.ESPEnabled);
            DrawStatus("F8", "Smite Everyone");
            DrawStatus("J", "Smite Nearest");
            DrawStatus("H", "Quick Heal");
            DrawStatus("F9", "Instant Win");
            DrawStatus("-/+", "Scale", Mathf.Approximately(ToggleStates.PlayerScale, 1f) ? "Normal" : ToggleStates.PlayerScale.ToString("F2"));
            DrawStatus("L", "FB Mode", ToggleStates.CurrentFireMode.ToString());
            DrawStatus("O", "MM Mode", ToggleStates.CurrentWardMode.ToString());
            DrawStatus("I", "FR Mode", ToggleStates.CurrentFrostMode.ToString());

            GUILayout.EndArea();
        }

        private static void InitStyles()
        {
            if (labelStyle != null) return;

            labelStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 15,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleLeft,
                richText = true,
                normal = { textColor = Color.white }
            };

            keyStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 15,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleLeft,
                richText = true,
                normal = { textColor = Color.yellow }
            };
        }

        private static void DrawStatus(string key, string label, bool state)
        {
            string value = state ? "<color=#00FF00>ON</color>" : "<color=red>OFF</color>";
            DrawStatus(key, label, value);
        }

        private static void DrawStatus(string key, string label, string value)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label($"{key}", keyStyle, GUILayout.Width(50));
            GUILayout.Label(label + ":", labelStyle, GUILayout.Width(130));
            GUILayout.Label(value, labelStyle);
            GUILayout.EndHorizontal();
        }

        private static void DrawStatus(string key, string label)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label($"{key}", keyStyle, GUILayout.Width(50));
            GUILayout.Label(label, labelStyle);
            GUILayout.EndHorizontal();
        }
    }
}