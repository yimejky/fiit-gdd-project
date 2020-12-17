using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private Canvas canvas;
    public void Awake()
    {
        Time.timeScale = 1;
        canvas = GetComponent<Canvas>();
        hide();
    }

    void Update()
    {
        if (Input.GetButtonDown("Pause"))
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

    public void Quit()
    {
        Debug.Log("Pause Menu Quit");
        Application.Quit();
    }
}
