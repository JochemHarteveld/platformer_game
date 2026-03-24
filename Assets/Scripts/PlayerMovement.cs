using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 12f;
    public AudioClip jumpSound;
    public AudioClip deathSound;
    public Sprite pickupSprite;

    private Rigidbody2D rb;
    private AudioSource audioSource;
    private bool isGrounded;
    private bool _isDying = false;
    private bool _hasKey = false;

    [HideInInspector] public Vector3 _spawnPosition;
    private SpriteRenderer _spriteRenderer;
    private Color _originalColor;
    private Sprite _originalSprite;

    private KeyPickup _collectedKey;
    private List<Door> _doors = new();

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originalColor = Color.yellow;
        if (_spriteRenderer != null)
        {
            _spriteRenderer.color = _originalColor;
            _originalSprite = _spriteRenderer.sprite;
        }
        if (_spawnPosition == Vector3.zero)
            _spawnPosition = transform.position;

        var col = GetComponent<Collider2D>();
        if (col != null)
            col.sharedMaterial = new PhysicsMaterial2D { friction = 0f, bounciness = 0f };
    }

    public void SetSpawnPoint(Vector3 pos)
    {
        _spawnPosition = pos;
        transform.position = pos;
        if (rb != null) rb.linearVelocity = Vector2.zero;
    }

    void Update()
    {
        if (_isDying) return;

        float move = 0f;
        var keyboard = Keyboard.current;
        if (keyboard != null)
        {
            if (keyboard.leftArrowKey.isPressed || keyboard.aKey.isPressed)
                move = -1f;
            else if (keyboard.rightArrowKey.isPressed || keyboard.dKey.isPressed)
                move = 1f;
        }

        var gamepad = Gamepad.current;
        if (gamepad != null && move == 0f)
            move = gamepad.leftStick.x.ReadValue();

        rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y);

        bool jumpPressed = (keyboard != null && keyboard.spaceKey.wasPressedThisFrame)
                        || (gamepad != null && gamepad.buttonSouth.wasPressedThisFrame);

        if (jumpPressed && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            if (audioSource != null && jumpSound != null)
                audioSource.PlayOneShot(jumpSound);
        }

        if (transform.position.y < -6f)
            Die();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;

        if (collision.gameObject.CompareTag("Enemy"))
            Die();
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = false;
    }

    void Die()
    {
        if (_isDying) return;
        _isDying = true;

        if (audioSource != null && deathSound != null)
            audioSource.PlayOneShot(deathSound);

        rb.linearVelocity = Vector2.zero;
        StartCoroutine(DieSequence());
    }

    IEnumerator DieSequence()
    {
        if (_spriteRenderer != null)
            _spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(0.4f);

        transform.position = _spawnPosition;
        rb.linearVelocity = Vector2.zero;
        isGrounded = false;

        if (_spriteRenderer != null)
        {
            _spriteRenderer.color = _originalColor;
            _spriteRenderer.sprite = _originalSprite;
        }

        if (_collectedKey != null)
        {
            _collectedKey.gameObject.SetActive(true);
            _collectedKey = null;
        }
        _hasKey = false;

        foreach (var door in _doors)
            door.Close();

        _isDying = false;
    }

    public bool HasKey => _hasKey;

    public void CollectKey(KeyPickup key)
    {
        _hasKey = true;
        _collectedKey = key;
        if (_spriteRenderer != null && pickupSprite != null)
            _spriteRenderer.sprite = pickupSprite;
    }

    public void UseKey()
    {
        _hasKey = false;
        if (_spriteRenderer != null)
            _spriteRenderer.sprite = _originalSprite;
    }

    public void RegisterDoor(Door door)
    {
        if (!_doors.Contains(door)) _doors.Add(door);
    }
}
