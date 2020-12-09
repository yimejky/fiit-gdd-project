using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeController : MonoBehaviour
{

    private StatsUpgrades stats;
    void Start()
    {
        stats = StatsUpgrades.Instance;
    }

    public void UpgradeAttribute(string name)
    {
        int upgradeResult = stats.UpgradeStat(name, 1);
        Debug.Log(name + " upgraded " + upgradeResult + " times");
    }
}
