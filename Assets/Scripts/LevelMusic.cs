using System;
using UnityEngine;

public class LevelMusic : MonoBehaviour
{
    [Serializable]
    public struct MusicLayer
    {
        public string    name;
        public AudioClip clip;
        [Range(0f, 1f)]
        public float     volume;
        public bool      muted;

        public MusicLayer(string name) : this()
        {
            this.name   = name;
            this.volume = 1f;
        }
    }

    [Header("Music Layers")]
    public MusicLayer[] layers = Array.Empty<MusicLayer>();

    [Header("Playback")]
    [Range(0f, 1f)]
    public float masterVolume = 0.5f;
    public bool  loop         = true;
    public bool  playOnStart  = true;

    AudioSource[] _sources;

    void Start()
    {
        _sources = new AudioSource[layers.Length];

        for (int i = 0; i < layers.Length; i++)
        {
            var src = gameObject.AddComponent<AudioSource>();
            src.clip        = layers[i].clip;
            src.volume      = layers[i].muted ? 0f : layers[i].volume * masterVolume;
            src.loop        = loop;
            src.playOnAwake = false;
            src.spatialBlend = 0f; // 2D audio
            _sources[i]     = src;
        }

        if (playOnStart)
            ScheduleAll();
    }

    // ── Public controls ──────────────────────────────────────────────────────

    public void Play()  => ScheduleAll();

    public void Stop()
    {
        if (_sources == null) return;
        foreach (var src in _sources) src.Stop();
    }

    public void Pause()
    {
        if (_sources == null) return;
        foreach (var src in _sources) src.Pause();
    }

    public void SetMasterVolume(float v)
    {
        masterVolume = Mathf.Clamp01(v);
        ApplyVolumes();
    }

    public void SetLayerVolume(int index, float v)
    {
        if (!ValidIndex(index)) return;
        layers[index].volume = Mathf.Clamp01(v);
        ApplyVolumes();
    }

    public void SetLayerMuted(int index, bool muted)
    {
        if (!ValidIndex(index)) return;
        layers[index].muted = muted;
        ApplyVolumes();
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    void ScheduleAll()
    {
        if (_sources == null) return;
        double startTime = AudioSettings.dspTime + 0.1;
        foreach (var src in _sources)
        {
            if (src.clip == null) continue;
            src.Stop();
            src.PlayScheduled(startTime);
        }
    }

    void ApplyVolumes()
    {
        if (_sources == null) return;
        for (int i = 0; i < _sources.Length; i++)
            _sources[i].volume = layers[i].muted ? 0f : layers[i].volume * masterVolume;
    }

    bool ValidIndex(int i) => _sources != null && i >= 0 && i < _sources.Length;
}
