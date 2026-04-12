using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class CutsceneManager : MonoBehaviour
{
    [System.Serializable]
    public class CutsceneEntry
    {
        public string name;
        public VideoClip clip;
    }

    [Header("Cutscene Library")]
    public CutsceneEntry[] cutscenes;

    [Header("References")]
    public VideoPlayer videoPlayer;
    public RawImage screenImage;
    //public GameObject uiRoot;

    [Header("Settings")]
    public bool hideUIWhenPlaying = true;

    void Start()
    {
        videoPlayer.renderMode = VideoRenderMode.RenderTexture;

        videoPlayer.prepareCompleted += OnPrepared;
        videoPlayer.loopPointReached += OnCutsceneEnd;

        //if (uiRoot != null)
        //    uiRoot.SetActive(false);
    }

    // Play by index
    public void Play(int index)
    {
        if (index < 0 || index >= cutscenes.Length) return;

        PlayClip(cutscenes[index].clip);
    }

    // Play by name 
    public void Play(string cutsceneName)
    {
        for (int i = 0; i < cutscenes.Length; i++)
        {
            if (cutscenes[i].name == cutsceneName)
            {
                PlayClip(cutscenes[i].clip);
                return;
            }
        }

        Debug.LogWarning("Cutscene not found: " + cutsceneName);
    }

    private void PlayClip(VideoClip clip)
    {
        if (clip == null) return;

        //if (uiRoot != null && hideUIWhenPlaying)
        //    uiRoot.SetActive(true);

        videoPlayer.Stop();
        videoPlayer.clip = clip;
        videoPlayer.Prepare();
    }

    private void OnPrepared(VideoPlayer vp)
    {
        if (screenImage != null)
            screenImage.texture = videoPlayer.texture;

        videoPlayer.Play();
    }

    private void OnCutsceneEnd(VideoPlayer vp)
    {
        StopCutscene();
    }

    public void StopCutscene()
    {
        videoPlayer.Stop();

        //if (uiRoot != null && hideUIWhenPlaying)
        //    uiRoot.SetActive(false);
    }

    public void ShowImage(Texture tex)
    {
        //if (uiRoot != null)
        //    uiRoot.SetActive(true);

        //if (screenImage != null)
        //    screenImage.texture = tex;
    }

    public void HideImage()
    {
        //if (uiRoot != null)
        //    uiRoot.SetActive(false);
    }
}