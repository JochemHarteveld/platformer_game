using UnityEngine;

public class PlatformGradient : MonoBehaviour
{
    static readonly Color TopColor    = new Color(0.1f, 1.0f, 0.95f); // neon cyan
    static readonly Color BottomColor = new Color(0.0f, 0.06f, 0.18f); // deep navy

    void Awake()
    {
        var tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
        tex.filterMode = FilterMode.Bilinear;
        tex.wrapMode   = TextureWrapMode.Clamp;
        tex.SetPixels(new Color[]
        {
            BottomColor, BottomColor, // row 0 (bottom)
            TopColor,    TopColor,    // row 1 (top)
        });
        tex.Apply();

        // PPU=2 on a 2×2 texture → sprite is exactly 1×1 world units,
        // so existing Transform scales continue to size platforms correctly.
        Sprite gradient = Sprite.Create(tex, new Rect(0, 0, 2, 2),
                                        new Vector2(0.5f, 0.5f), 2f);

        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Ground"))
        {
            SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
            if (sr == null) continue;
            sr.sprite = gradient;
            sr.color  = Color.white;
        }
    }
}
