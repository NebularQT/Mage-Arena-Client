using UnityEngine;
using FishNet.Object;
using UnityEngine.UIElements;
using System.Reflection;
using MageArenaClient;

public class SimpleESP : MonoBehaviour
{
    PlayerMovement localPlayer;
    public Color enemyBoxColor = new Color(1f, 0f, 0f, 0.20f);
    public Color teamBoxColor = new Color(0f, 1f, 0f, 0.20f);

    void OnGUI()
    {
        if (!ToggleStates.ESPEnabled)
            return;

        if (localPlayer == null)
            localPlayer = FindLocalPlayer();

        if (localPlayer == null)
            return;
        foreach (var player in Object.FindObjectsByType<PlayerMovement>(FindObjectsSortMode.None))
        {
            if (player == null || player.isDead || player == localPlayer) continue;

            //player box to screen relative postition
            var cc = player.GetComponent<CharacterController>();
            float playerHeight = cc != null ? cc.height : 2f;
            float scale = player.transform.localScale.y;

            Vector3 headWorldPos = player.transform.position + Vector3.up * playerHeight * scale;
            Vector3 footWorldPos = player.transform.position;

            Vector3 headViewportPos = Camera.main.WorldToViewportPoint(headWorldPos);
            Vector3 footViewportPos = Camera.main.WorldToViewportPoint(footWorldPos);

            if (headViewportPos.z <= 0 || footViewportPos.z <= 0)
                continue;

            float headX = headViewportPos.x * Screen.width;
            float headY = (1f - headViewportPos.y) * Screen.height;

            float footX = footViewportPos.x * Screen.width;
            float footY = (1f - footViewportPos.y) * Screen.height;

            float height = footY - headY;
            float width = height / 2f;

            Rect boxRect = new Rect(headX - width / 2f, headY, width, height);

            //team colors
            bool isTeammate = player.playerTeam == localPlayer.playerTeam;
            Color boxColor = isTeammate ? teamBoxColor : enemyBoxColor;

            DrawBox(boxRect, boxColor);

            //hp bar
            float hp = GetHealth(player);
            float fullBarWidth = boxRect.width;
            float barHeight = 4f;
            float barY = boxRect.yMax + 2f;

            //background
            GUI.color = Color.black;
            GUI.DrawTexture(new Rect(boxRect.x, barY, fullBarWidth, barHeight), Texture2D.whiteTexture);

            Rect healthBarRect = new Rect(boxRect.x, boxRect.yMax + 2, boxRect.width, 4);

            //update to health
            GUI.color = GetHealthColor(hp);

            GUI.DrawTexture(healthBarRect, Texture2D.whiteTexture);

            //name
            string username = player.playername ?? "Player";
            Vector2 nameSize = GUI.skin.label.CalcSize(new GUIContent(username));
            float nameX = boxRect.center.x - nameSize.x / 2f;
            float nameY = boxRect.yMin - nameSize.y - 2f;

            GUI.color = Color.white;
            GUI.Label(new Rect(nameX, nameY, nameSize.x, nameSize.y), username);
        }

        GUI.color = Color.white;
    }

    void DrawBox(Rect rect, Color color)
    {
        Color prevColor = GUI.color;

        GUI.color = color;
        GUI.DrawTexture(rect, Texture2D.whiteTexture);

        GUI.color = Color.black;
        GUI.DrawTexture(new Rect(rect.x, rect.y, rect.width, 1), Texture2D.whiteTexture);
        GUI.DrawTexture(new Rect(rect.x, rect.y + rect.height - 1, rect.width, 1), Texture2D.whiteTexture);
        GUI.DrawTexture(new Rect(rect.x, rect.y, 1, rect.height), Texture2D.whiteTexture);
        GUI.DrawTexture(new Rect(rect.x + rect.width - 1, rect.y, 1, rect.height), Texture2D.whiteTexture);

        GUI.color = prevColor;
    }

    PlayerMovement FindLocalPlayer()
    {
        foreach (var pm in Object.FindObjectsByType<PlayerMovement>(FindObjectsSortMode.None))
        {
            var netObj = pm.GetComponent<NetworkObject>();
            if (netObj != null && netObj.IsOwner)
                return pm;
        }
        return null;
    }

    Color GetHealthColor(float hp)
    {
        if (hp >= 100 && hp <= 150)
        {
            float t = (hp - 100) / 50f;
            return Color.Lerp(Color.green, new Color(1f, 0f, 1f),  t); 
        }
        else if (hp >= 0 && hp < 100)
        {
            float t = hp / 100f;
            return Color.Lerp(Color.red, Color.green, t); 
        }
        else if (hp >= 150)
        {
            return new Color(1f, 0f, 1f); 
        }
        else
        {
            return Color.red; 
        }
    }
    private float GetHealth(PlayerMovement player)
    {
        if (player == null) return -1f;

        var healthbar2Field = typeof(PlayerMovement).GetField("Healthbar2", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        if (healthbar2Field == null) return -1f;

        var healthbar2 = healthbar2Field.GetValue(player);
        if (healthbar2 == null) return -1f;

        var lerperField = healthbar2.GetType().GetField("lerper", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        if (lerperField == null) return -1f;

        var value = lerperField.GetValue(healthbar2);
        if (value is float lerper)
        {
            return lerper * 10f; //make it relative to actual health from graphical modifier
        }

        return -1f;
    }
}