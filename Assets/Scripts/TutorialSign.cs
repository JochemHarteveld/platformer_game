using UnityEngine;

public class TutorialSign : MonoBehaviour
{
    public string message = "";
    public Font modernFont;
    [Header("Glow Animation")]
    public float glowSpeed = 2f;
    public float minIntensity = 0.6f;
    public float maxIntensity = 1f;

    private GUIStyle _style;
    private Color _baseNeonColor = new Color(0f, 1f, 0.9f, 1f); // Neon cyan

    void Start()
    {
        // Create neon text style
        _style = new GUIStyle();
        _style.fontSize = 36;
        _style.fontStyle = FontStyle.Normal;
        _style.normal.textColor = _baseNeonColor;
        _style.alignment = TextAnchor.MiddleCenter;
        _style.padding = new RectOffset(10, 10, 10, 10);

        // Apply modern font if assigned
        if (modernFont != null)
        {
            _style.font = modernFont;
        }
    }

    void OnGUI()
    {
        if (Camera.main == null) return;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        if (screenPos.z <= 0) return;
        float x = screenPos.x;
        float y = Screen.height - screenPos.y - 100; // Moved up by 100 pixels

        // Animate glow intensity using sine wave
        float glowIntensity = Mathf.Lerp(minIntensity, maxIntensity, (Mathf.Sin(Time.time * glowSpeed) + 1f) / 2f);
        Color glowColor = _baseNeonColor * glowIntensity;
        glowColor.a = 1f; // Keep alpha at full

        // Draw glow effect (shadow layers for glow)
        _style.normal.textColor = glowColor * 0.5f;
        GUI.Label(new Rect(x - 198, y - 23, 400, 50), message, _style);
        GUI.Label(new Rect(x - 202, y - 27, 400, 50), message, _style);

        // Draw main neon text on top
        _style.normal.textColor = glowColor;
        GUI.Label(new Rect(x - 200, y - 25, 400, 50), message, _style);
    }
}

