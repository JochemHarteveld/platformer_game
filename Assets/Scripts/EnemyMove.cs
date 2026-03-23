using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public float speed = 2f;
    public float leftLimit;
    public float rightLimit;

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        if (transform.position.x > rightLimit || transform.position.x < leftLimit)
        {
            speed *= -1;
        }
    }
}