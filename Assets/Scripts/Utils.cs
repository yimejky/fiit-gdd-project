using System;

public static class Utils
{
    public static T RandomEnumValue<T>()
    {
        var values = Enum.GetValues(typeof(T));
        int random = UnityEngine.Random.Range(1, values.Length);
        return (T)values.GetValue(random);
    }
}
