using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class EnemyMove : MonoBehaviour
{
    public float speed = 2f;
    public float leftLimit;
    public float rightLimit;

    void Start()
    {
        GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 1f, 1f); // Neon magenta
    }

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        if (transform.position.x > rightLimit || transform.position.x < leftLimit)
        {
            speed *= -1;
        }
    }
}