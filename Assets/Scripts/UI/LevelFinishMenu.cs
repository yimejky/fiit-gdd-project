using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelFinishMenu : Menu
{
    public string nextLevelScene;
    public int newPoints = 3;

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

        SceneManager.LoadScene(nextLevelScene);
    }
}

public abstract class Menu : MonoBehaviour
{
    public abstract void Display();

    public abstract void Hide();
}
