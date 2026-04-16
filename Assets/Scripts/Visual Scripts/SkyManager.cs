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
        RenderSettings.fog = stages[stage].fog;
        RenderSettings.fogMode = stages[stage].fogMode;
        RenderSettings.fogColor = stages[stage].fogColor;
        RenderSettings.fogEndDistance = stages[stage].fogEndDistance;
        RenderSettings.fogStartDistance = stages[stage].fogStartDistance;
        RenderSettings.skybox = stages[stage].stage_skybox;
    }

    void ExitSky(int stage)
    {


    }
}

