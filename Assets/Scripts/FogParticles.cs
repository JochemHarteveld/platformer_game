using UnityEngine;

/// <summary>
/// Creates a fog particle effect in the foreground.
/// Attach to a GameObject with a ParticleSystem component.
/// </summary>
[RequireComponent(typeof(ParticleSystem))]
public class FogParticles : MonoBehaviour
{
    [Header("Fog Settings")]
    public int maxParticles = 100;
    public float fogRiseSpeed = 1.5f;
    public float fogSize = 5f;
    public Color fogColor = new Color(0.7f, 0.7f, 0.7f, 0.2f);

    [Header("Area Settings")]
    public float spawnWidth = 25f;
    public float bottomOffset = 5f; // How far below camera to spawn

    private ParticleSystem ps;
    private ParticleSystem.MainModule mainModule;
    private ParticleSystem.EmissionModule emissionModule;
    private ParticleSystem.ShapeModule shapeModule;
    private ParticleSystem.ColorOverLifetimeModule colorModule;
    private ParticleSystem.SizeOverLifetimeModule sizeModule;
    private ParticleSystem.VelocityOverLifetimeModule velocityModule;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        SetupFogParticles();
    }

    void SetupFogParticles()
    {
        // Main module
        mainModule = ps.main;
        mainModule.startLifetime = 12f;
        mainModule.startSpeed = 0f; // No initial speed, controlled by velocity module
        mainModule.startSize = new ParticleSystem.MinMaxCurve(fogSize * 0.8f, fogSize * 1.5f);
        mainModule.startColor = fogColor;
        mainModule.maxParticles = maxParticles;
        mainModule.simulationSpace = ParticleSystemSimulationSpace.World;
        mainModule.loop = true;

        // Emission
        emissionModule = ps.emission;
        emissionModule.rateOverTime = 15f;

        // Shape - emit from horizontal line at bottom
        shapeModule = ps.shape;
        shapeModule.enabled = true;
        shapeModule.shapeType = ParticleSystemShapeType.Box;
        shapeModule.scale = new Vector3(spawnWidth, 0.5f, 1f);

        // Velocity - rise upward from bottom
        velocityModule = ps.velocityOverLifetime;
        velocityModule.enabled = true;
        velocityModule.space = ParticleSystemSimulationSpace.World;
        velocityModule.x = new ParticleSystem.MinMaxCurve(-0.3f, 0.3f); // Slight horizontal drift
        velocityModule.y = new ParticleSystem.MinMaxCurve(fogRiseSpeed * 0.8f, fogRiseSpeed * 1.2f); // Rise upward

        // Color over lifetime - fade in and out
        colorModule = ps.colorOverLifetime;
        colorModule.enabled = true;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] {
                new GradientColorKey(Color.white, 0.0f),
                new GradientColorKey(Color.white, 1.0f)
            },
            new GradientAlphaKey[] {
                new GradientAlphaKey(0.0f, 0.0f),
                new GradientAlphaKey(0.25f, 0.15f),
                new GradientAlphaKey(0.25f, 0.85f),
                new GradientAlphaKey(0.0f, 1.0f)
            }
        );
        colorModule.color = gradient;

        // Size over lifetime - grow as it rises for blur effect
        sizeModule = ps.sizeOverLifetime;
        sizeModule.enabled = true;
        AnimationCurve sizeCurve = new AnimationCurve();
        sizeCurve.AddKey(0.0f, 0.8f);
        sizeCurve.AddKey(0.3f, 1.0f);
        sizeCurve.AddKey(0.7f, 1.3f);
        sizeCurve.AddKey(1.0f, 1.5f);
        sizeModule.size = new ParticleSystem.MinMaxCurve(1f, sizeCurve);

        // Renderer settings - absolute foreground
        var renderer = ps.GetComponent<ParticleSystemRenderer>();
        renderer.sortingOrder = 1000; // Very high value for absolute foreground
        renderer.renderMode = ParticleSystemRenderMode.Billboard;

        // For best blur effect: In Unity, assign a soft, feathered sprite to the particle material
    }

    void Update()
    {
        // Follow camera and spawn from bottom
        if (Camera.main != null)
        {
            Vector3 newPos = Camera.main.transform.position;
            newPos.y -= bottomOffset; // Position below camera view
            newPos.z = 15f; // Very close to camera - absolute foreground
            transform.position = newPos;
        }
    }
}
