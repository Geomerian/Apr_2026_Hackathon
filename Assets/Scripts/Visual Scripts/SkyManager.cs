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
        public bool isFog;
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
        switch (stage) {
            default:
                RenderSettings.fog = false;
                break;
            case 1: // Denial
                RenderSettings.fog = true;
                RenderSettings.fogMode = FogMode.Linear;
                RenderSettings.fogColor = new Color32(53,17,144,204);
                RenderSettings.fogEndDistance = 30f;
                RenderSettings.fogStartDistance = 15f;
                RenderSettings.skybox = stages[1].stage_skybox;
                break;
            case 2: // Anger
                break;
            case 3: // Bargaining
                break;
            case 4: // Depression
                break;
            case 5: // Acceptance
                break;
        }
    }

    void ExitSky(int stage)
    {
        switch (stage) {
        
            case 1: // Denial
                
                break;
            case 2: // Anger
                break;
            case 3: // Bargaining
                break;
            case 4: // Depression
                break;
            case 5: // Acceptance
                break;
        }

    }
}

