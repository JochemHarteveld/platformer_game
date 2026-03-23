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
        GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0.8f, 1f); // Bright neon pink
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
