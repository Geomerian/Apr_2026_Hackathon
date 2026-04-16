using UnityEngine;

public class SkyManager : MonoBehaviour
{
    public static SkyManager Instance;

    [SerializeField]
    int currentStage = 1;

    [System.Serializable]
    public class StageSky
    {
        public string name;
        public int stageNumber;

        [Header("Skybox")]
        public bool fog;
        public FogMode fogMode;
        public Color32 fogColor;
        public float fogStartDistance;
        public float fogEndDistance;
        public Material stage_skybox;

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
        EnterSky(stage);

    }

    void EnterSky(int stage) {
        int stageNum = stage - 1; //bandaid will make better when unifying levelstates later
        RenderSettings.fog = stages[stageNum].fog;
        RenderSettings.fogMode = stages[stageNum].fogMode;
        RenderSettings.fogColor = stages[stageNum].fogColor;
        RenderSettings.fogEndDistance = stages[stageNum].fogEndDistance;
        RenderSettings.fogStartDistance = stages[stageNum].fogStartDistance;
        RenderSettings.skybox = stages[stageNum].stage_skybox;
    }

    void ExitSky(int stage)
    {


    }
}

