using UnityEngine;
using UnityEngine.UI;


public class MainMenu : MonoBehaviour
{
    private LoadingScreen loadingScreen;
    private GameState state;

    void Start()
    {
        loadingScreen = GameObject.Find("LoadingScreenCanvas").GetComponent<LoadingScreen>();
        state = GameStatePersistence.LoadState();
        Button button = transform.Find("Panel").Find("ContinueButton").GetComponent<Button>();
        button.interactable = state != null;
    }

    public void Continue()
    {
        Debug.Log("Continue game");
        loadingScreen.LoadScene(state.currentSceneName);
    }

    public void NewGame()
    {
        Debug.Log("New game");
        StatsUpgrades.NewInstance();
        loadingScreen.LoadScene(Constants.FIRST_LEVEL_SCENE);
    }

    public void Quit()
    {
        Debug.Log("Menu Quit");
        Application.Quit();
    }
}
