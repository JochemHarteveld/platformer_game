using UnityEngine;

/// <summary>
/// Manages background music for each level.
/// Attach this to a GameObject in each scene and assign the music track for that level.
/// The music will automatically play when the scene loads.
/// </summary>
public class LevelMusic : MonoBehaviour
{
    [Header("Level Music Settings")]
    public AudioClip musicTrack;
    [Range(0f, 1f)]
    public float volume = 0.5f;
    public bool loop = true;
    public bool playOnStart = true;

    private AudioSource audioSource;

    void Start()
    {
        // Get or add AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Configure AudioSource
        audioSource.clip = musicTrack;
        audioSource.volume = volume;
        audioSource.loop = loop;
        audioSource.playOnAwake = false;

        // Play music if enabled
        if (playOnStart && musicTrack != null)
        {
            audioSource.Play();
        }
    }

    /// <summary>
    /// Play the music track
    /// </summary>
    public void Play()
    {
        if (audioSource != null && musicTrack != null)
        {
            audioSource.Play();
        }
    }

    /// <summary>
    /// Stop the music
    /// </summary>
    public void Stop()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }

    /// <summary>
    /// Pause the music
    /// </summary>
    public void Pause()
    {
        if (audioSource != null)
        {
            audioSource.Pause();
        }
    }

    /// <summary>
    /// Change the volume
    /// </summary>
    public void SetVolume(float newVolume)
    {
        volume = Mathf.Clamp01(newVolume);
        if (audioSource != null)
        {
            audioSource.volume = volume;
        }
    }
}
