using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public AudioClip pickupSound;
    public float pickupVolume = 1f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        var player = other.GetComponent<PlayerMovement>();
        if (player != null) player.CollectKey(this);

        if (pickupSound != null)
            AudioSource.PlayClipAtPoint(pickupSound, transform.position, pickupVolume);

        gameObject.SetActive(false);
    }
}
