using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 12f;
    public AudioClip jumpSound;
    public AudioClip deathSound;

    private Rigidbody2D rb;
    private AudioSource audioSource;
    private bool isGrounded;

    private Vector3 _spawnPosition;
    private SpriteRenderer _spriteRenderer;
    private Color _originalColor;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originalColor = _spriteRenderer != null ? _spriteRenderer.color : Color.white;
        _spawnPosition = transform.position;
    }

    void Update()
    {
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
        if (audioSource != null && deathSound != null)
            audioSource.PlayOneShot(deathSound);

        rb.linearVelocity = Vector2.zero;
        transform.position = _spawnPosition;
        isGrounded = false;

        if (_spriteRenderer != null)
            StartCoroutine(FlashRed());
    }

    IEnumerator FlashRed()
    {
        _spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.4f);
        _spriteRenderer.color = _originalColor;
    }
}
