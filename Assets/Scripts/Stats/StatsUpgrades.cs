using System;
using System.Collections.Generic;

public sealed class StatsUpgrades
{
    private List<StatsObserver> observers;
    private static StatsUpgrades instance = new StatsUpgrades();

    public Dictionary<string, int> stats = new Dictionary<string, int>();

    // Explicit static constructor to tell C# compiler
    // not to mark type as beforefieldinit
    static StatsUpgrades()
    {
    }

    private StatsUpgrades(bool newGame = false)
    {
        GameState state = GameStatePersistence.LoadState();
        if (state != null && !newGame)
        {
            stats = state.stats;
        }
        else
        {
            stats.Add("health", 0);
            stats.Add("sword", 0);
            stats.Add("bow", 0);
            stats.Add("points", 5);
        }

        observers = new List<StatsObserver>();
    }

    public static StatsUpgrades Instance
    {
        get
        {
            return instance;
        }
    }

    public static StatsUpgrades NewInstance()
    {
        instance = new StatsUpgrades(true);
        return instance;
    }

    public int GetStat(String name)
    {
        return stats[name];
    }

    public int UpgradeStat(String name, int amount)
    {
        int addition = Math.Min(amount, stats["points"]);
        stats[name] += addition;
        stats["points"] -= addition;
        foreach (var observer in observers)
            observer.StatsUpdate(name, addition);
        return addition;
    }

    public void Subscribe(StatsObserver observer)
    {
        if (!observers.Contains(observer))
        {
            observers.Add(observer);

            // FIXME if we expand the number of stats, make it loop over keys
            observer.StatsUpdate("health", stats["health"]);
            observer.StatsUpdate("sword", stats["sword"]);
            observer.StatsUpdate("bow", stats["bow"]);
        }
    }

    public void Unsubscribe(StatsObserver observer)
    {
        if (observers.Contains(observer))
        {
            observers.Remove(observer);
        }
    }
}

public interface StatsObserver
{
    void StatsUpdate(string name, int value);
}