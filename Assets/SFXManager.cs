using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;

    [Header("Pool")]
    public int poolSize = 15;
    private AudioSource[] pool;

    void Awake()
    {
        Instance = this;
        BuildPool();
    }

    void BuildPool()
    {
        pool = new AudioSource[poolSize];

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = new GameObject("SFX_" + i);
            obj.transform.parent = transform;

            AudioSource src = obj.AddComponent<AudioSource>();
            src.spatialBlend = 1f;
            src.playOnAwake = false;

            pool[i] = src;
        }
    }

    public void PlaySFX(AudioClip clip, Vector3 position, float volume = 1f)
    {
        if (clip == null) return;

        AudioSource src = GetFreeSource();
        if (src == null) return;

        src.transform.position = position;
        src.clip = clip;
        src.volume = volume;

        src.Play();
    }

    AudioSource GetFreeSource()
    {
        foreach (var s in pool)
        {
            if (!s.isPlaying)
                return s;
        }

        return null;
    }
}