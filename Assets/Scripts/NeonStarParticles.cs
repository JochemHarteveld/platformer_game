using UnityEngine;

/// <summary>
/// Creates neon colored falling star particles.
/// Attach to a GameObject with a ParticleSystem component.
/// </summary>
[RequireComponent(typeof(ParticleSystem))]
public class NeonStarParticles : MonoBehaviour
{
    [Header("Star Settings")]
    public int maxParticles = 50;
    public float fallSpeed = 3f;
    public float starSize = 0.5f;

    [Header("Neon Colors")]
    public Color[] neonColors = new Color[]
    {
        new Color(0f, 1f, 1f, 1f),      // Neon cyan
        new Color(1f, 0f, 1f, 1f),      // Neon magenta
        new Color(1f, 0f, 0.8f, 1f),    // Neon pink
        new Color(0.1f, 1f, 0.1f, 1f),  // Neon green
        new Color(1f, 1f, 0f, 1f),      // Neon yellow
    };

    [Header("Area Settings")]
    public float spawnWidth = 25f;
    public float spawnHeight = 20f;
    public float spawnAboveCamera = 5f;

    private ParticleSystem ps;
    private ParticleSystem.MainModule mainModule;
    private ParticleSystem.EmissionModule emissionModule;
    private ParticleSystem.ShapeModule shapeModule;
    private ParticleSystem.VelocityOverLifetimeModule velocityModule;
    private ParticleSystem.ColorOverLifetimeModule colorModule;
    private ParticleSystem.TrailModule trailModule;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        SetupNeonStarParticles();
    }

    void SetupNeonStarParticles()
    {
        // Main module
        mainModule = ps.main;
        mainModule.startLifetime = new ParticleSystem.MinMaxCurve(5f, 8f);
        mainModule.startSpeed = new ParticleSystem.MinMaxCurve(fallSpeed * 0.5f, fallSpeed * 1.5f);
        mainModule.startSize = new ParticleSystem.MinMaxCurve(starSize * 0.5f, starSize * 1.5f);
        mainModule.startRotation = new ParticleSystem.MinMaxCurve(0f, 360f * Mathf.Deg2Rad);
        mainModule.maxParticles = maxParticles;
        mainModule.simulationSpace = ParticleSystemSimulationSpace.World;
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
        emissionModule.rateOverTime = 5f;

        // Shape - emit from top of screen
        shapeModule = ps.shape;
        shapeModule.enabled = true;
        shapeModule.shapeType = ParticleSystemShapeType.Box;
        shapeModule.scale = new Vector3(spawnWidth, 1f, 1f);

        // Velocity - diagonal falling motion
        velocityModule = ps.velocityOverLifetime;
        velocityModule.enabled = true;
        velocityModule.space = ParticleSystemSimulationSpace.World;
        velocityModule.x = new ParticleSystem.MinMaxCurve(-2f, 2f);
        velocityModule.y = new ParticleSystem.MinMaxCurve(-fallSpeed * 0.8f, -fallSpeed * 1.2f);

        // Color over lifetime - shimmer effect
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
                new GradientAlphaKey(1.0f, 0.1f),
                new GradientAlphaKey(0.8f, 0.9f),
                new GradientAlphaKey(0.0f, 1.0f)
            }
        );
        colorModule.color = lifetimeGradient;

        // Trail module - create falling star trail
        trailModule = ps.trails;
        trailModule.enabled = true;
        trailModule.ratio = 1f;
        trailModule.lifetime = 0.3f;
        trailModule.minVertexDistance = 0.1f;
        trailModule.textureMode = ParticleSystemTrailTextureMode.Stretch;
        trailModule.worldSpace = false;
        trailModule.dieWithParticles = true;

        Gradient trailGradient = new Gradient();
        trailGradient.SetKeys(
            new GradientColorKey[] {
                new GradientColorKey(Color.white, 0.0f),
                new GradientColorKey(Color.white, 1.0f)
            },
            new GradientAlphaKey[] {
                new GradientAlphaKey(1.0f, 0.0f),
                new GradientAlphaKey(0.0f, 1.0f)
            }
        );
        trailModule.colorOverLifetime = trailGradient;

        // Renderer settings
        var renderer = ps.GetComponent<ParticleSystemRenderer>();
        renderer.sortingOrder = 50; // Behind fog (which is 100), in front of background
        renderer.renderMode = ParticleSystemRenderMode.Billboard;
    }

    void Update()
    {
        // Follow camera to keep stars spawning above view
        if (Camera.main != null)
        {
            Vector3 newPos = Camera.main.transform.position;
            newPos.y += spawnAboveCamera;
            newPos.z = 0f; // Behind fog
            transform.position = newPos;
        }
    }
}
