using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelFinishMenu : MonoBehaviour
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

    public void Hide()
    {
        Time.timeScale = 1;
        canvas.enabled = false;
    }

    public void Display()
    {
        Time.timeScale = 0;
        canvas.enabled = true;
    }

    public void ResetLevel()
    {
        // TODO load stats
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextLevel()
    {
        // TODO save stats 
        SceneManager.LoadScene(nextLevelScene);
    }
}
