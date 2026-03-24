using UnityEngine;

/// <summary>
/// Enemy that patrols vertically between bottomLimit and topLimit.
/// Colour: magenta (set via SpriteRenderer in Start).
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class EnemyVertical : MonoBehaviour
{
    public float speed = 2f;
    public float bottomLimit;
    public float topLimit;

    void Start()
    {
        GetComponent<SpriteRenderer>().color = new Color(2f, 0f, 1.6f, 1f); // HDR neon pink
    }

    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);

        if (transform.position.y > topLimit || transform.position.y < bottomLimit)
        {
            speed *= -1;
        }
    }
}
