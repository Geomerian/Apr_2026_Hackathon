using System.Collections;
using UnityEngine;

public class AmbianceManager : MonoBehaviour
{
    public static AmbianceManager Instance;

    [System.Serializable]
    public class StageAmbiance
    {
        public string name;

        [Header("Music Layers (looping)")]
        public AudioClip[] musicLayers; // up to 5

        [Header("Random SFX")]
        public AudioClip[] sfxClips;

        public float sfxIntervalMin = 5f;
        public float sfxIntervalMax = 15f;
        public float sfxRadius = 15f;
    }

    [Header("Stages")]
    public StageAmbiance[] stages;

    [Header("Audio Sources")]
    public AudioSource[] musicSources; // assign 3–5 in inspector

    [Header("SFX Pool")]
    public int sfxPoolSize = 10;
    private AudioSource[] sfxPool;

    private int currentStage = -1;
    private Transform player;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        player = GameManager.Instance.currentPlayer?.transform;

        // create SFX pool
        sfxPool = new AudioSource[sfxPoolSize];

        for (int i = 0; i < sfxPoolSize; i++)
        {
            GameObject obj = new GameObject("SFX_" + i);
            obj.transform.parent = transform;

            AudioSource src = obj.AddComponent<AudioSource>();
            src.spatialBlend = 1f; // 3D sound
            src.playOnAwake = false;

            sfxPool[i] = src;
        }

        StartCoroutine(StageWatcher());
    }

    IEnumerator StageWatcher()
    {
        while (true)
        {
            int gmStage = GameManager.Instance.currentStage;

            if (gmStage != currentStage)
            {
                ApplyStage(gmStage);
                currentStage = gmStage;
            }

            yield return new WaitForSeconds(1f);
        }
    }

    // APPLY STAGE AMBIANCE
    void ApplyStage(int stageIndex)
    {
        if (stageIndex < 0 || stageIndex >= stages.Length) return;

        StopAllCoroutines(); // stop previous SFX loop
        StartCoroutine(StageWatcher()); // restart watcher

        StageAmbiance stage = stages[stageIndex];

        // 🎵 MUSIC LAYERS
        for (int i = 0; i < musicSources.Length; i++)
        {
            if (i < stage.musicLayers.Length && stage.musicLayers[i] != null)
            {
                musicSources[i].clip = stage.musicLayers[i];
                musicSources[i].loop = true;
                musicSources[i].Play();
            }
            else
            {
                musicSources[i].Stop();
            }
        }

        // START RANDOM SFX
        StartCoroutine(RandomSFXLoop(stage));
    }

    IEnumerator RandomSFXLoop(StageAmbiance stage)
    {
        while (true)
        {
            float wait = Random.Range(stage.sfxIntervalMin, stage.sfxIntervalMax);
            yield return new WaitForSeconds(wait);

            PlayRandomSFX(stage);
        }
    }

    void PlayRandomSFX(StageAmbiance stage)
    {
        if (stage.sfxClips.Length == 0 || player == null) return;

        AudioClip clip = stage.sfxClips[Random.Range(0, stage.sfxClips.Length)];

        AudioSource src = GetFreeSFXSource();
        if (src == null) return;

        Vector3 randomPos = player.position + Random.insideUnitSphere * stage.sfxRadius;

        src.transform.position = randomPos;
        src.clip = clip;
        src.Play();
    }

    AudioSource GetFreeSFXSource()
    {
        for (int i = 0; i < sfxPool.Length; i++)
        {
            if (!sfxPool[i].isPlaying)
                return sfxPool[i];
        }

        return null; // pool exhausted
    }
}