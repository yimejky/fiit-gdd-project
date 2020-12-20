using UnityEngine;

public class LevelFinishMenu : Menu
{
    public string nextLevelScene;
    public int newPoints = 3;

    private LoadingScreen loadingScreen;
    private Canvas canvas;

    void Start()
    {
        loadingScreen = GameObject.Find("LoadingScreenCanvas").GetComponent<LoadingScreen>();
        canvas = GetComponent<Canvas>();
    }
    
    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            canvas.enabled = false;
        }
    }

    public override void Hide()
    {
        Time.timeScale = 1;
        canvas.enabled = false;
    }

    public override void Display()
    {
        Debug.Log("FInish menu pause time");
        Time.timeScale = 0;
        canvas.enabled = true;
    }

    public void ResetLevel()
    {
        Utils.ResetLevel();
    }

    public void NextLevel()
    {
        StatsUpgrades.Instance.AddPoints(newPoints);
        GameState state = new GameState
        {
            currentSceneName = nextLevelScene,
            stats = StatsUpgrades.Instance.stats
        };
        GameStatePersistence.SaveState(state);

        loadingScreen.GetComponent<LoadingScreen>().LoadScene(nextLevelScene);
        // SceneManager.LoadScene(nextLevelScene);
    }
}

public abstract class Menu : MonoBehaviour
{
    public abstract void Display();

    public abstract void Hide();
}
