using System;
using UnityEngine;

/// <summary>
/// Place one of these in every level scene.
/// Position this GameObject where the player should spawn.
/// Optionally override music and/or background for this level.
/// </summary>
public class LevelSetup : MonoBehaviour
{
    [Header("Music (leave empty to keep current music)")]
    public LevelMusic.MusicLayer[] musicLayers = Array.Empty<LevelMusic.MusicLayer>();

    [Header("Background (leave null to keep current background)")]
    public Sprite backgroundSprite;
}
