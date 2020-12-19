using UnityEngine;
using UnityEngine.SceneManagement;

public class HeroDiedMenu : Menu
{
    private Canvas canvas;

    void Start()
    {
        canvas = GetComponent<Canvas>();
    }

    public void Awake()
    {
        Time.timeScale = 0;
        Debug.Log("HeroDiedMenu awake: zero time scale");
        PauseMenu.locked = true;
    }

    public void ResetLevel()
    {
        Debug.Log("HeroDiedMenu: reset level");
        PauseMenu.locked = false;
        Time.timeScale = 1;
        Utils.ResetLevel();
    }

    public void Quit()
    {
        Debug.Log("HeroDiedMenu: return to Main Menu");
        PauseMenu.locked = false;
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public override void Display()
    {
        Debug.Log("HeroDiedMenu: Display");
        Time.timeScale = 0;
        PauseMenu.locked = false;
        canvas.enabled = true;
    }

    public override void Hide()
    {
        throw new System.NotImplementedException();
    }
}
