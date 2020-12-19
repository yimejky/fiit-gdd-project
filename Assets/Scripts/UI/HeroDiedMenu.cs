using UnityEngine;
using UnityEngine.SceneManagement;

public class HeroDiedMenu : MonoBehaviour
{
    public void Awake()
    {
        Time.timeScale = 0;
        PauseMenu.locked = true;
    }

    public void ResetLevel()
    {
        Debug.Log("HeroDiedMenu reset level");
        PauseMenu.locked = false;
        Utils.ResetLevel();
    }

    public void Quit()
    {
        PauseMenu.locked = false;
        Debug.Log("HeroDiedMenu return to Main Menu");
        SceneManager.LoadScene("MainMenu");
    }
}
