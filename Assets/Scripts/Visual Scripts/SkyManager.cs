using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;

public class SkyManager : MonoBehaviour
{
    public static SkyManager Instance;
   
    [SerializeField]
    private int currentStage = 1;

    [System.Serializable]
    public class StageSky
    {
        public string name;
        public int stageNumber;

        [Header("Fog Settings")]
        public bool fog;
        public FogMode fogMode;
        public Color32 fogColor;
        public float fogStartDistance;
        public float fogEndDistance;
        public Material stage_skybox;

        [Header("Sky Light Collection")]
        public GameObject skyLights;
    }

    [Header("Stages")]
    public StageSky[] stages;

    void Start() {

    }

    private void Awake()
    {
        if (Instance != null) {
            Destroy(this);
        }

        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetSky(int stage) {
        ExitSky(currentStage);
        currentStage = stage;
        EnterSky(currentStage);

    }

    void EnterSky(int stage) {
        int stageNum = stage - 1; //bandaid will make better when unifying levelstates later
        RenderSettings.fog = stages[stageNum].fog;
        RenderSettings.fogMode = stages[stageNum].fogMode;
        RenderSettings.fogColor = stages[stageNum].fogColor;
        RenderSettings.fogEndDistance = stages[stageNum].fogEndDistance;
        RenderSettings.fogStartDistance = stages[stageNum].fogStartDistance;
        RenderSettings.skybox = stages[stageNum].stage_skybox;

        if (stages[stageNum].skyLights != null)
        {
            stages[stageNum].skyLights.SetActive(true);
        }
    }

    void ExitSky(int stage)
    {
        int stageNum = stage - 1; //bandaid will make better when unifying levelstates later

        stages[stageNum].skyLights.SetActive(false);

    }

    public void EnterLightning()
    {
        RenderSettings.fogStartDistance = 50f;
        RenderSettings.fogEndDistance = 100f;
    }

    public void ExitLightning()
    {
        RenderSettings.fogStartDistance = stages[currentStage].fogStartDistance;
        RenderSettings.fogEndDistance = stages[currentStage].fogEndDistance;
    }
}

