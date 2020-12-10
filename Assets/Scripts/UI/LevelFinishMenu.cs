using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelFinishMenu : Menu
{
    public string nextLevelScene;

    private Canvas canvas;

    void Start()
    {
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
        Time.timeScale = 0;
        canvas.enabled = true;
    }

    public void ResetLevel()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == Constants.FIRST_LEVEL_SCENE)
        {
            StatsUpgrades.NewInstance();
        }
        else
        {
            StatsUpgrades.Instance.stats = GameStatePersistence.LoadState().stats;
        }

        SceneManager.LoadScene(sceneName);
    }

    public void NextLevel()
    {
        GameState state = new GameState();
        state.currentSceneName = nextLevelScene;
        state.stats = StatsUpgrades.Instance.stats;
        GameStatePersistence.SaveState(state);

        SceneManager.LoadScene(nextLevelScene);
    }
}

public abstract class Menu : MonoBehaviour
{
    public abstract void Display();

    public abstract void Hide();
}
