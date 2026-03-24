using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class EnemyMove : MonoBehaviour
{
    public float speed = 2f;
    public float leftLimit;
    public float rightLimit;

    private SpriteRenderer _sr;

    void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        _sr.color = new Color(2f, 0f, 2f, 1f); // HDR neon magenta
    }

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        if (transform.position.x > rightLimit || transform.position.x < leftLimit)
            speed *= -1;

        _sr.flipX = speed < 0;
    }
}