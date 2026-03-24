using UnityEngine;

public class Door : MonoBehaviour
{
    public AudioClip unlockSound;
    public float unlockVolume = 1f;
    public Sprite openSprite;

    private Collider2D _col;
    private SpriteRenderer _sr;
    private Sprite _closedSprite;

    void Start()
    {
        _col = GetComponent<Collider2D>();
        _sr  = GetComponent<SpriteRenderer>();
        if (_sr != null) _closedSprite = _sr.sprite;

        var player = GameObject.FindWithTag("Player");
        if (player != null)
            player.GetComponent<PlayerMovement>().RegisterDoor(this);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        var player = collision.gameObject.GetComponent<PlayerMovement>();
        if (player != null && player.HasKey)
        {
            player.UseKey();
            Open();
        }
    }

    public void Open()
    {
        if (_col != null) _col.enabled = false;
        if (unlockSound != null)
            AudioSource.PlayClipAtPoint(unlockSound, transform.position, unlockVolume);
        if (_sr != null && openSprite != null)
            _sr.sprite = openSprite;
    }

    public void Close()
    {
        if (_col != null) _col.enabled = true;
        if (_sr != null && _closedSprite != null)
            _sr.sprite = _closedSprite;
    }
}
