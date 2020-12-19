using System;
using System.Reflection;
using UnityEngine;

public class GameConfigManager : MonoBehaviour
{
    public GameConfig gameConfig;

    public T GetConfig<T>(string propName)
    {
        FieldInfo info = gameConfig.GetType().GetField(propName);

        try
        {
            T value = (T) info.GetValue(gameConfig);
            return value;
        } catch (Exception e)
        {
            Debug.Log($"ERROR '{propName}', {typeof(T)} '{e}'");
        }

        return default(T);
    }

    public static GameConfigManager Get()
    {
        return GameObject.Find("ScriptableObjectReference").GetComponent<GameConfigManager>();
    }
}
