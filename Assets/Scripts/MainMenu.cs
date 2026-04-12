using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public int levelChosen = 1;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void SetLevel(int level)
    {
        levelChosen = level;
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}
