using UnityEngine;

public class PlayerSFX : MonoBehaviour
{
    [System.Serializable]
    public class StageFootstepEntry
    {
        public int stage;
        public AudioClip[] clips;
    }

    [Header("Stage Footsteps (Manual Mapping)")]
    public StageFootstepEntry[] stageFootsteps;

    [Header("Water Footsteps")]
    public AudioClip[] waterFootsteps;

    [Header("Settings")]
    public float stepCooldown = 0.4f;
    public float volume = 1f;

    public bool inWater = false;

    private float stepTimer;

    void Update()
    {
        stepTimer -= Time.deltaTime;
    }

    public void TryPlayFootstep()
    {
        if (stepTimer > 0f) return;

        stepTimer = stepCooldown;
        PlayFootstep();
    }

    void PlayFootstep()
    {
        AudioClip clip = null;

        // Water override
        if (inWater && waterFootsteps.Length > 0)
        {
            clip = waterFootsteps[Random.Range(0, waterFootsteps.Length)];
        }
        else
        {
            int currentStage = GameManager.Instance.currentStage;

            // 🔍 Find matching stage manually
            for (int i = 0; i < stageFootsteps.Length; i++)
            {
                if (stageFootsteps[i].stage == currentStage)
                {
                    var set = stageFootsteps[i].clips;

                    if (set.Length > 0)
                        clip = set[Random.Range(0, set.Length)];

                    break;
                }
            }
        }

        if (clip != null)
        {
            SFXManager.Instance.PlaySFX(clip, transform.position, volume);
        }
    }
}