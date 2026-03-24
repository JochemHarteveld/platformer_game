using System.Collections;
using UnityEngine;

public class CrumblingPlatform : MonoBehaviour
{
    [Header("Sounds")]
    public AudioClip activateSound;
    [Range(0f, 2f)] public float activateSoundDelay = 0f;
    [Range(0f, 1f)] public float activateSoundVolume = 1f;

    public AudioClip disappearSound;
    [Range(0f, 2f)] public float disappearSoundDelay = 0f;
    [Range(0f, 1f)] public float disappearSoundVolume = 1f;

    public AudioClip respawnSound;
    [Range(0f, 2f)] public float respawnSoundDelay = 0f;
    [Range(0f, 1f)] public float respawnSoundVolume = 1f;

    private SpriteRenderer _sr;
    private Collider2D _col;
    private AudioSource _audio;
    private Vector3 _originPos;
    private bool _crumbling = false;
    private Sprite _spriteOrange;
    private Sprite _spriteRed;

    void Start()
    {
        _sr    = GetComponent<SpriteRenderer>();
        _col   = GetComponent<Collider2D>();
        _audio = GetComponent<AudioSource>();
        _originPos = transform.position;

        _spriteOrange = MakeGradientSprite(new Color(1.0f, 0.6f, 0.0f), new Color(0.55f, 0.22f, 0.0f));
        _spriteRed    = MakeGradientSprite(new Color(1.0f, 0.1f, 0.0f), new Color(0.45f, 0.0f,  0.0f));

        if (_sr != null)
        {
            _sr.sprite = _spriteOrange;
            _sr.color  = Color.white;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (_crumbling) return;
        if (!collision.gameObject.CompareTag("Player")) return;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y < -0.5f)
            {
                _crumbling = true;
                StartCoroutine(CrumbleSequence());
                return;
            }
        }
    }

    IEnumerator CrumbleSequence()
    {
        // Activate sound
        StartCoroutine(PlayDelayed(activateSound, activateSoundDelay, activateSoundVolume));

        // Phase 1: red-orange flicker (0.5s)
        float elapsed = 0f;
        bool toggle = false;
        while (elapsed < 0.5f)
        {
            if (_sr != null) _sr.sprite = toggle ? _spriteRed : _spriteOrange;
            toggle = !toggle;
            yield return new WaitForSeconds(0.05f);
            elapsed += 0.05f;
        }

        // Disappear sound
        StartCoroutine(PlayDelayed(disappearSound, disappearSoundDelay, disappearSoundVolume));

        // Phase 2: glitch jitter (0.5s)
        elapsed = 0f;
        while (elapsed < 0.5f)
        {
            if (_sr != null) _sr.sprite = toggle ? _spriteRed : _spriteOrange;
            toggle = !toggle;
            transform.position = _originPos + new Vector3(
                Random.Range(-0.06f, 0.06f),
                Random.Range(-0.04f, 0.04f),
                0f
            );
            yield return new WaitForSeconds(0.05f);
            elapsed += 0.05f;
        }

        transform.position = _originPos;
        if (_col != null) _col.enabled = false;
        if (_sr != null)  _sr.enabled  = false;

        // Phase 3: wait hidden
        yield return new WaitForSeconds(5f);

        // Respawn sound
        StartCoroutine(PlayDelayed(respawnSound, respawnSoundDelay, respawnSoundVolume));

        // Phase 4: respawn flicker (1s)
        if (_sr != null)
        {
            _sr.sprite = _spriteOrange;
            _sr.color  = Color.white;
        }
        elapsed = 0f;
        bool visible = false;
        while (elapsed < 1f)
        {
            if (_sr != null) _sr.enabled = visible;
            visible = !visible;
            yield return new WaitForSeconds(0.08f);
            elapsed += 0.08f;
        }

        if (_sr != null)  _sr.enabled  = true;
        if (_col != null) _col.enabled = true;
        _crumbling = false;
    }

    IEnumerator PlayDelayed(AudioClip clip, float delay, float volume)
    {
        if (delay > 0f) yield return new WaitForSeconds(delay);
        if (_audio != null && clip != null) _audio.PlayOneShot(clip, volume);
    }

    static Sprite MakeGradientSprite(Color top, Color bottom)
    {
        var tex = new Texture2D(2, 2, TextureFormat.RGBA32, false)
        {
            filterMode = FilterMode.Bilinear,
            wrapMode   = TextureWrapMode.Clamp
        };
        tex.SetPixels(new[] { bottom, bottom, top, top });
        tex.Apply();
        return Sprite.Create(tex, new Rect(0, 0, 2, 2), new Vector2(0.5f, 0.5f), 2f);
    }
}
