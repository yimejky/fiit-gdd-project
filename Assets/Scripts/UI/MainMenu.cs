using UnityEngine;
using UnityEngine.UI;


public class MainMenu : MonoBehaviour
{
    public Canvas loadingScreen;
    private GameState state;

    void Start()
    {
        state = GameStatePersistence.LoadState();
        Button button = transform.Find("ContinueButton").GetComponent<Button>();
        button.interactable = state != null;
    }

    public void Continue()
    {
        Debug.Log("Continue game");
        loadingScreen.GetComponent<LoadingScreen>().LoadScene(state.currentSceneName);
    }

    public void NewGame()
    {
        Debug.Log("New game");
        StatsUpgrades.NewInstance();
        loadingScreen.GetComponent<LoadingScreen>().LoadScene(Constants.FIRST_LEVEL_SCENE);
    }

    public void Quit()
    {
        Debug.Log("Menu Quit");
        Application.Quit();
    }
}
