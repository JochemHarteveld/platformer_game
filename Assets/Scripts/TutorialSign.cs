using UnityEngine;

public class TutorialSign : MonoBehaviour
{
    public string message = "";

    private GUIStyle _style;

    void Start()
    {
        _style = new GUIStyle();
        _style.fontSize = 32;
        _style.fontStyle = FontStyle.Bold;
        _style.normal.textColor = Color.yellow;
        _style.alignment = TextAnchor.MiddleCenter;
    }

    void OnGUI()
    {
        if (Camera.main == null) return;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        if (screenPos.z <= 0) return;
        float x = screenPos.x;
        float y = Screen.height - screenPos.y;
        GUI.Label(new Rect(x - 200, y - 25, 400, 50), message, _style);
    }
}
