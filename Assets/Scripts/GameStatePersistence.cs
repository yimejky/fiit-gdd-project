using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using System;

public static class GameStatePersistence
{
    public static void SaveState(GameState state)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/gamestate.data";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, state);
        stream.Close();
    }

    public static GameState LoadState()
    {
        string path = Application.persistentDataPath + "/gamestate.data";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameState state = formatter.Deserialize(stream) as GameState;
            stream.Close();
            return state;
        }
        else
        {
            Debug.Log("Game state not found");
            return null;
        }
    }
}

[Serializable]
public class GameState
{
    public Dictionary<string, int> stats;
    public string currentSceneName;
}
