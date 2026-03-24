using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Singleton that persists across all scenes.
/// Assign the persistent objects in the Inspector (Level1 only).
/// Every subsequent level just needs a LevelSetup component in the scene.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Persistent Objects — assign in Level1")]
    public GameObject player;
    public GameObject mainCamera;
    public GameObject levelMusicObject;
    public GameObject skyBackground;
    public GameObject neonStarsFar;
    public GameObject neonStarsMid;
    public GameObject neonStarsNear;
    public GameObject fog;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        Persist(player);
        Persist(mainCamera);
        Persist(levelMusicObject);
        Persist(skyBackground);
        Persist(neonStarsFar);
        Persist(neonStarsMid);
        Persist(neonStarsNear);
        Persist(fog);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy() => SceneManager.sceneLoaded -= OnSceneLoaded;

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Destroy any duplicate Player/Camera that new scenes may contain
        DestroyDuplicates<PlayerMovement>(player);
        DestroyDuplicates<Camera>(mainCamera);

        var setup = FindFirstObjectByType<LevelSetup>();
        if (setup == null) return;

        // Move persistent player to this level's spawn point
        if (player != null)
            player.GetComponent<PlayerMovement>()?.SetSpawnPoint(setup.transform.position);

        // Override music if this level specifies layers
        if (setup.musicLayers is { Length: > 0 } && levelMusicObject != null)
        {
            var music = levelMusicObject.GetComponent<LevelMusic>();
            if (music != null)
            {
                music.Stop();
                music.layers = setup.musicLayers;
                music.Initialize();
            }
        }

        // Override background sprite if specified
        if (setup.backgroundSprite != null && skyBackground != null)
        {
            var sr = skyBackground.GetComponent<SpriteRenderer>();
            if (sr != null) sr.sprite = setup.backgroundSprite;
        }
    }

    static void Persist(GameObject go) { if (go != null) DontDestroyOnLoad(go); }

    void DestroyDuplicates<T>(GameObject persistent) where T : Component
    {
        if (persistent == null) return;
        foreach (var c in FindObjectsByType<T>(FindObjectsSortMode.None))
            if (c.gameObject != persistent) Destroy(c.gameObject);
    }
}
