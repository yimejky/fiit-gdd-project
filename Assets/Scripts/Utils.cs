using System;
using UnityEngine.SceneManagement;

public static class Utils
{
    public static T RandomEnumValue<T>()
    {
        var values = Enum.GetValues(typeof(T));
        int random = UnityEngine.Random.Range(1, values.Length);
        return (T)values.GetValue(random);
    }

    public static void ResetLevel()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == Constants.FIRST_LEVEL_SCENE)
        {
            StatsUpgrades.NewInstance();
        }
        else
        {
            StatsUpgrades.Instance.stats = GameStatePersistence.LoadState().stats;
        }

        SceneManager.LoadScene(sceneName);
    }
}
