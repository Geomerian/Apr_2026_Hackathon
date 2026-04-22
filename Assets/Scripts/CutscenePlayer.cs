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
        // filename for webgl builds
        public string webglFilename;
    }

    [Header("Cutscene Library")]
    public CutsceneEntry[] cutscenes;

    [Header("References")]
    public VideoPlayer videoPlayer;
    public RawImage screenImage;
    //public GameObject uiRoot;

    [Header("Settings")]
    public bool hideUIWhenPlaying = true;

    [Header("Skip Settings")]
    public KeyCode skipKey = KeyCode.Escape;
    public bool allowSkip = true;

    void Start()
    {
        videoPlayer.renderMode = VideoRenderMode.RenderTexture;
        videoPlayer.prepareCompleted += OnPrepared;
        videoPlayer.loopPointReached += OnCutsceneEnd;
        //if (uiRoot != null)
        //    uiRoot.SetActive(false);
    }

    void Update()
    {
        if (allowSkip && videoPlayer.isPlaying && Input.GetKeyDown(skipKey))
        {
            StopCutscene();
        }
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
                #if UNITY_WEBGL
                string  url = System.IO.Path.Combine(Application.streamingAssetsPath, cutscenes[i].webglFilename);
                PlayURL(url);
                #endif 

                #if !UNITY_WEBGL
                PlayClip(cutscenes[i].clip);
                #endif

                return;
            }
        }
        Debug.LogWarning("Cutscene not found: " + cutsceneName);
    }

    private void PlayClip(VideoClip clip)
    {
        if (clip == null) return;
        screenImage.gameObject.SetActive(true);
        //if (uiRoot != null && hideUIWhenPlaying)
        //    uiRoot.SetActive(true);
        videoPlayer.Stop();
        videoPlayer.clip = clip;
        videoPlayer.Prepare();
    }

    private void PlayURL(string url)
    {
        if (string.IsNullOrEmpty(url)) return;
        screenImage.gameObject.SetActive(true);
        //if (uiRoot != null && hideUIWhenPlaying)
        //    uiRoot.SetActive(true);
        videoPlayer.Stop();
        videoPlayer.url = url;
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
        screenImage.gameObject.SetActive(false);
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