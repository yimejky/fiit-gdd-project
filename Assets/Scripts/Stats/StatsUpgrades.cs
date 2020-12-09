using System;
using System.Collections.Generic;

public sealed class StatsUpgrades
{
    private static readonly StatsUpgrades instance = new StatsUpgrades();

    private Dictionary<string, int> stats = new Dictionary<string, int>();

    // Explicit static constructor to tell C# compiler
    // not to mark type as beforefieldinit
    static StatsUpgrades()
    {
    }

    private StatsUpgrades()
    {
        // TODO load stats from config/saved game

        stats.Add("health", 0);
        stats.Add("sword", 0);
        stats.Add("bow", 0);
        stats.Add("points", 5);
    }

    public static StatsUpgrades Instance
    {
        get
        {
            return instance;
        }
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
        return addition;
    }
}