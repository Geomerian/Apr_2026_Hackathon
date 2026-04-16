using System;
using System.Collections;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Randomly triggers a directional light to simulate lightning strikes.
/// Assign a Directional Light to the <see cref="lightSource"/> field, configure
/// one or more <see cref="LightningSettings"/> entries, then hit Play.
/// </summary>
public class RandomLightning : MonoBehaviour
{
    // -------------------------------------------------------------------------
    // Data Types
    // -------------------------------------------------------------------------

    [Serializable]
    public class RotationRange
    {
        [Tooltip("Min/Max Euler degrees for each axis.")]
        public Vector2 x = new Vector2(-10f, 10f);
        public Vector2 y = new Vector2(0f, 360f);
        public Vector2 z = new Vector2(-10f, 10f);

        public Quaternion Sample()
        {
            return Quaternion.Euler(
                UnityEngine.Random.Range(x.x, x.y),
                UnityEngine.Random.Range(y.x, y.y),
                UnityEngine.Random.Range(z.x, z.y)
            );
        }
    }

    [Serializable]
    public class LightningSettings
    {
        [Tooltip("Label shown in the Inspector for convenience.")]
        public string label = "Strike";

        [Tooltip("Raw weight — higher = more likely. Does NOT need to sum to 1.")]
        [Min(0f)]
        public float weight = 1f;

        [Tooltip("Rotation applied to the directional light during this strike.")]
        public RotationRange rotation = new RotationRange();

        [Tooltip("How long (seconds) the strike lasts. A random value in [min, max] is chosen each time.")]
        public Vector2 durationRange = new Vector2(0.08f, 0.25f);

        [Tooltip("Intensity over normalised strike time (0 = start, 1 = end). " +
                 "Peak value drives the light's maximum intensity for this strike.")]
        public AnimationCurve intensityCurve = DefaultCurve();

        [Tooltip("Maximum intensity the curve peak maps to (curve is evaluated 0-1 then multiplied here).")]
        [Min(0f)]
        public float peakIntensity = 8f;

        public float SampleDuration() =>
            UnityEngine.Random.Range(durationRange.x, durationRange.y);

        private static AnimationCurve DefaultCurve()
        {
            // Quick flash: ramp up, brief hold, ramp down
            var curve = new AnimationCurve(
                new Keyframe(0.00f, 0f, 0f,  20f),
                new Keyframe(0.05f, 1f, 0f,   0f),
                new Keyframe(0.15f, 0.8f, 0f, 0f),
                new Keyframe(0.50f, 0.6f, 0f, 0f),
                new Keyframe(1.00f, 0f, -2f,  0f)
            );
            return curve;
        }
    }

    // -------------------------------------------------------------------------
    // Inspector Fields
    // -------------------------------------------------------------------------

    [Header("Light Source")]
    [Tooltip("The Directional Light that will flash. Can be anywhere in the scene.")]
    public Light lightSource;

    [Header("Strike Interval")]
    [Tooltip("Seconds between strikes (random value chosen each time).")]
    public Vector2 intervalRange = new Vector2(2f, 8f);

    [Header("Lightning Presets")]
    [Tooltip("Add multiple entries with different weights to vary your storm.")]
    public LightningSettings[] settings = { new LightningSettings() };

    // -------------------------------------------------------------------------
    // Runtime
    // -------------------------------------------------------------------------

    private float   _originalIntensity;
    private Quaternion _originalRotation;
    private bool    _striking;

    private void Awake()
    {
        if (lightSource == null)
        {
            Debug.LogWarning("[RandomLightning] No light source assigned — searching for a Directional Light in the scene.");
            lightSource = FindFirstDirectionalLight();
        }

        if (lightSource != null)
        {
            _originalIntensity = lightSource.intensity;
            _originalRotation  = lightSource.transform.rotation;
            lightSource.gameObject.SetActive(false);
        }
    }

    private void OnEnable()  => StartCoroutine(LightningLoop());
    private void OnDisable() => StopAllCoroutines();

    // -------------------------------------------------------------------------
    // Core Loop
    // -------------------------------------------------------------------------

    private IEnumerator LightningLoop()
    {
        while (true)
        {
            float wait = UnityEngine.Random.Range(intervalRange.x, intervalRange.y);
            yield return new WaitForSeconds(wait);

            if (lightSource == null || settings == null || settings.Length == 0)
                continue;

            LightningSettings chosen = PickWeighted();
            if (chosen == null) continue;

            yield return StartCoroutine(Strike(chosen));
        }
    }

    private IEnumerator Strike(LightningSettings s)
    {
        lightSource.gameObject.SetActive(true);
        _striking = true;

        SkyManager.Instance.EnterLightning();

        // Apply rotation
        Quaternion strikeRotation = s.rotation.Sample();
        lightSource.transform.rotation = strikeRotation;

        float duration = s.SampleDuration();
        float elapsed  = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            lightSource.intensity = s.intensityCurve.Evaluate(t) * s.peakIntensity;
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Restore
        lightSource.intensity         = _originalIntensity;
        lightSource.transform.rotation = _originalRotation;

        SkyManager.Instance.ExitLightning();

        _striking = false;
        lightSource.gameObject.SetActive(false);
    }

    // -------------------------------------------------------------------------
    // Weighted Random Selection
    // -------------------------------------------------------------------------

    private LightningSettings PickWeighted()
    {
        float total = 0f;
        foreach (var s in settings) total += Mathf.Max(0f, s.weight);

        if (total <= 0f) return settings[0];

        float roll = UnityEngine.Random.Range(0f, total);
        float cumulative = 0f;

        foreach (var s in settings)
        {
            cumulative += Mathf.Max(0f, s.weight);
            if (roll <= cumulative) return s;
        }

        return settings[settings.Length - 1];
    }

    // -------------------------------------------------------------------------
    // Helpers
    // -------------------------------------------------------------------------

    private static Light FindFirstDirectionalLight()
    {
        foreach (var l in FindObjectsByType<Light>(FindObjectsSortMode.None))
            if (l.type == LightType.Directional) return l;
        return null;
    }

    // -------------------------------------------------------------------------
    // Editor Gizmo — draws the rotation range as a cone in the Scene view
    // -------------------------------------------------------------------------
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (settings == null) return;

        foreach (var s in settings)
        {
            if (s == null) continue;
            Vector3 origin = transform.position;

            // Draw a few sample rays to visualise the rotation spread
            Handles.color = new Color(1f, 0.92f, 0.3f, 0.4f);
            const int samples = 24;
            for (int i = 0; i < samples; i++)
            {
                Quaternion rot = s.rotation.Sample();
                Handles.DrawLine(origin, origin + rot * Vector3.forward * 3f);
            }
        }
    }
#endif
}