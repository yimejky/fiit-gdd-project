using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private Canvas canvas;
    public static bool locked;

    public void Awake()
    {
        Time.timeScale = 1;
        canvas = GetComponent<Canvas>();
        hide();
        locked = false;
    }

    void Update()
    {
        if (Input.GetButtonDown("Pause") && !locked)
        {
            Time.timeScale = Time.timeScale == 1 ? 0 : 1;
            if (Time.timeScale == 0)
            {
                show();
            }
            else if (Time.timeScale == 1)
            {
                hide();
            }
        }
    }

    void hide()
    {
        canvas.enabled = false;
        transform.Find("PauseMenu")?.gameObject.SetActive(false);
    }

    void show()
    {
        canvas.enabled = true;
        transform.Find("PauseMenu")?.gameObject.SetActive(true);
    }

    public void ResetLevel()
    {
        Utils.ResetLevel();
    }

    public void Quit()
    {
        Debug.Log("Pause Menu return to Main Menu");
        SceneManager.LoadScene("MainMenu");
    }
}
