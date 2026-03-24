using UnityEngine;

/// <summary>
/// Creates neon colored falling star particles with parallax depth.
/// Attach to a GameObject with a ParticleSystem component.
/// Create 3 instances with different parallaxFactor values for depth effect.
/// </summary>
[RequireComponent(typeof(ParticleSystem))]
public class NeonStarParticles : MonoBehaviour
{
    [Header("Star Settings")]
    public int maxParticles = 50;
    public float moveSpeed = 3f;
    public float starSize = 0.2f;

    [Header("Neon Colors (HDR for glow)")]
    public float glowIntensity = 2f;
    public Color[] neonColors = new Color[]
    {
        new Color(0f, 2f, 2f, 1f),      // Neon cyan (HDR)
        new Color(2f, 0f, 2f, 1f),      // Neon magenta (HDR)
        new Color(2f, 0f, 1.6f, 1f),    // Neon pink (HDR)
        new Color(0.2f, 2f, 0.2f, 1f),  // Neon green (HDR)
        new Color(2f, 2f, 0f, 1f),      // Neon yellow (HDR)
    };

    [Header("Area Settings")]
    public float spawnWidth = 35f;
    public float spawnHeight = 25f;
    public float spawnAboveCamera = 8f;

    [Header("Parallax")]
    [Tooltip("1 = tracks camera (distant sky feel — barely moves on screen). 0 = world-fixed (part of the map feel — drifts off as player moves). Use 0.9/0.5/0.1 for far/mid/near layers.")]
    public float parallaxFactor = 0.5f;

    [Header("Rendering")]
    [Tooltip("Use negative values to appear behind all gameplay elements. E.g. -200 (far), -150 (mid), -100 (near).")]
    public int sortingOrder = -150;
    [Tooltip("Assign StarMaterial (Additive blend) for the light-emitting glow effect.")]
    public Material starMaterial;

    private ParticleSystem ps;
    private ParticleSystem.MainModule mainModule;
    private ParticleSystem.EmissionModule emissionModule;
    private ParticleSystem.ShapeModule shapeModule;
    private ParticleSystem.VelocityOverLifetimeModule velocityModule;
    private ParticleSystem.ColorOverLifetimeModule colorModule;
    private ParticleSystem.TrailModule trailModule;

    private Vector3 _lastCameraPos;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        SetupNeonStarParticles();

        if (Camera.main != null)
        {
            _lastCameraPos = Camera.main.transform.position;
            // Initialize position at camera
            Vector3 startPos = Camera.main.transform.position;
            startPos.z = transform.position.z; // preserve configured Z depth
            transform.position = startPos;
        }
    }

    void SetupNeonStarParticles()
    {
        // Main module
        mainModule = ps.main;
        mainModule.startLifetime = new ParticleSystem.MinMaxCurve(5f, 9f);
        mainModule.startSpeed = new ParticleSystem.MinMaxCurve(moveSpeed * 0.6f, moveSpeed * 1.4f);
        mainModule.startSize = new ParticleSystem.MinMaxCurve(starSize * 0.85f, starSize * 1.3f);
        mainModule.startRotation = new ParticleSystem.MinMaxCurve(0f, 360f * Mathf.Deg2Rad);
        mainModule.startRotation3D = true;
        mainModule.maxParticles = maxParticles;
        mainModule.simulationSpace = ParticleSystemSimulationSpace.Local;
        mainModule.loop = true;

        // Random color from neon palette
        Gradient gradient = new Gradient();
        GradientColorKey[] colorKeys = new GradientColorKey[neonColors.Length];
        for (int i = 0; i < neonColors.Length; i++)
        {
            colorKeys[i] = new GradientColorKey(neonColors[i], i / (float)(neonColors.Length - 1));
        }
        gradient.SetKeys(colorKeys, new GradientAlphaKey[] {
            new GradientAlphaKey(1.0f, 0.0f),
            new GradientAlphaKey(1.0f, 1.0f)
        });
        mainModule.startColor = gradient;

        // Emission
        emissionModule = ps.emission;
        emissionModule.rateOverTime = maxParticles / 7f; // scale rate with particle count

        // Shape - wide box ABOVE camera, tilted to emit diagonally downward (shooting star direction)
        shapeModule = ps.shape;
        shapeModule.enabled = true;
        shapeModule.shapeType = ParticleSystemShapeType.Box;
        shapeModule.scale = new Vector3(spawnWidth, spawnHeight, 1f);
        shapeModule.position = new Vector3(0f, spawnAboveCamera, 0f); // offset above camera center
        shapeModule.randomDirectionAmount = 0f;   // consistent direction, not random scatter
        shapeModule.rotation = new Vector3(0f, 0f, -25f); // tilt ~25° so stars fall diagonally

        // Velocity over lifetime - slight downward gravity drift for natural feel
        velocityModule = ps.velocityOverLifetime;
        velocityModule.enabled = true;
        velocityModule.space = ParticleSystemSimulationSpace.World;
        velocityModule.y = new ParticleSystem.MinMaxCurve(-0.5f); // gentle extra downward pull

        // Color over lifetime - fade in, sustain, fade out (shimmer)
        colorModule = ps.colorOverLifetime;
        colorModule.enabled = true;
        Gradient lifetimeGradient = new Gradient();
        lifetimeGradient.SetKeys(
            new GradientColorKey[] {
                new GradientColorKey(Color.white, 0.0f),
                new GradientColorKey(Color.white, 1.0f)
            },
            new GradientAlphaKey[] {
                new GradientAlphaKey(0.0f, 0.0f),
                new GradientAlphaKey(1.0f, 0.12f),
                new GradientAlphaKey(0.85f, 0.88f),
                new GradientAlphaKey(0.0f, 1.0f)
            }
        );
        colorModule.color = lifetimeGradient;

        // Trail module - tapered soft trail matching particle color
        trailModule = ps.trails;
        trailModule.enabled = true;
        trailModule.ratio = 1f;
        trailModule.lifetime = new ParticleSystem.MinMaxCurve(0.15f, 0.35f);
        trailModule.minVertexDistance = 0.05f;
        trailModule.textureMode = ParticleSystemTrailTextureMode.Stretch;
        trailModule.worldSpace = false;
        trailModule.dieWithParticles = true;
        trailModule.inheritParticleColor = true;

        // Trail tapers from wide at particle to point — creates soft comet-tail look
        AnimationCurve trailWidthCurve = new AnimationCurve(
            new Keyframe(0f, 1f),    // full width at particle head
            new Keyframe(1f, 0f)    // tapers to nothing at tail
        );
        trailModule.widthOverTrail = new ParticleSystem.MinMaxCurve(starSize * 1.5f, trailWidthCurve);

        // Trail alpha fades out toward tail; color inherits from particle
        Gradient trailGradient = new Gradient();
        trailGradient.SetKeys(
            new GradientColorKey[] {
                new GradientColorKey(Color.white, 0.0f),
                new GradientColorKey(Color.white, 1.0f)
            },
            new GradientAlphaKey[] {
                new GradientAlphaKey(0.9f, 0.0f),
                new GradientAlphaKey(0.0f, 1.0f)
            }
        );
        trailModule.colorOverLifetime = trailGradient;

        // Renderer settings
        var renderer = ps.GetComponent<ParticleSystemRenderer>();
        renderer.sortingOrder = sortingOrder;
        renderer.renderMode = ParticleSystemRenderMode.Billboard;

        if (starMaterial != null)
        {
            renderer.material = starMaterial;
            renderer.trailMaterial = starMaterial;
        }
    }

    void Update()
    {
        if (Camera.main == null) return;

        Vector3 cameraPos = Camera.main.transform.position;
        Vector3 delta = cameraPos - _lastCameraPos;

        // X: parallax — high factor = follows camera (distant sky), low factor = world-fixed (map element)
        // Y: always follows camera fully so emitter never drifts off-screen vertically
        transform.position += new Vector3(delta.x * parallaxFactor, delta.y, 0f);

        _lastCameraPos = cameraPos;
    }
}
