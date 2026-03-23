using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(SpriteRenderer))]
public class Finish : MonoBehaviour
{
    public int nextSceneIndex = 1;

    void Start()
    {
        GetComponent<SpriteRenderer>().color = new Color(0.1f, 1f, 0.1f, 1f); // Bright neon green
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }
}