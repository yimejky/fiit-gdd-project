using System;
using UnityEngine;
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
            try
            {
                StatsUpgrades.Instance.stats = GameStatePersistence.LoadState().stats;
            }
            catch {
                StatsUpgrades.NewInstance();
            }
        }

        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1;
    }
}
