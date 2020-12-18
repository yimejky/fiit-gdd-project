using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// inspired by https://gamedevbeginner.com/how-to-load-a-new-scene-in-unity-with-a-loading-screen/
public class LoadingScreen : MonoBehaviour
{
    public Slider progressBar;

    AsyncOperation loadingOperation;
    Canvas canvas;

    private void Start()
    {
        progressBar.value = 0;
        canvas = GetComponent<Canvas>();
    }

    public void LoadScene(string sceneToLoad)
    {
        canvas.enabled = true;
        loadingOperation = SceneManager.LoadSceneAsync(sceneToLoad);
    }

    private void Update()
    {
        if (canvas.enabled)
        {
            progressBar.value = Mathf.Clamp01(loadingOperation.progress / 1.0f);
        }
    }
}