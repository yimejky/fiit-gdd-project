using UnityEngine;
using UnityEngine.UI;

public class StatsPanel : MonoBehaviour
{
    public string attributeName;
    
    private StatsUpgrades stats;
    private Text text;

    void Start()
    {
        stats = StatsUpgrades.Instance;
        text = GetComponent<Text>();
    }

    void Update()
    {
        text.text = stats.GetStat(attributeName).ToString();
    }
}
