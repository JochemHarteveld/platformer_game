using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SkyBackground : MonoBehaviour
{
    Camera cam;

    void Start()
    {
        cam = Camera.main;
        float height = cam.orthographicSize * 2f;
        float width = height * cam.aspect;
        var sr = GetComponent<SpriteRenderer>();
        var spriteSize = sr.sprite.bounds.size;
        transform.localScale = new Vector3(width / spriteSize.x, height / spriteSize.y, 1f);
    }

    void LateUpdate()
    {
        var pos = cam.transform.position;
        transform.position = new Vector3(pos.x, pos.y, transform.position.z);
    }
}
