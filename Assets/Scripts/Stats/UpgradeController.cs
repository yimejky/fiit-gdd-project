using UnityEngine;

public class UpgradeController : MonoBehaviour
{
    public void UpgradeAttribute(string name)
    {
        int upgradeResult = StatsUpgrades.Instance.UpgradeStat(name, 1);
        // Debug.Log(name + " upgraded " + upgradeResult + " times");
    }
}
