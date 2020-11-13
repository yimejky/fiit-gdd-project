using UnityEngine;


public class MainMenu : MonoBehaviour
{
    public Canvas loadingScreen;

    public void Play()
    {
        Debug.Log("Menu Play");
        loadingScreen.GetComponent<LoadingScreen>().LoadScene("Sandbox");
    }

    public void Quit()
    {
        Debug.Log("Menu Quit");
        Application.Quit();
    }
}
