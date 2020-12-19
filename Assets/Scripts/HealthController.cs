using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public class DamageDealEvent : UnityEvent<GameObject, GameObject, int>
{
}

public class HealthController : MonoBehaviour, IUpgradable
{
    public bool displayHealthBar = true;
    public string configName;
    public DamageDealEvent HealthUpdateEvent;

    private int maxHealth;
    private int actualHealth;
    private HealthControllerConfig healthConfig => GameConfigManager.Get().GetConfig<HealthControllerConfig>(configName);

    // healtbar settings
    public Color color = Color.green;
    public Vector3 healtBarOffset = new Vector3(0, 1.5f);
    public GameObject healthBar;

    private void Start()
    {
        maxHealth = healthConfig.maxHealth;
        actualHealth = healthConfig.startHealth;

        Upgrade(StatsUpgrades.Instance.GetStat("health") * PlayerController.playerControllerConfig.healthCoefficient);

        if (HealthUpdateEvent == null)
        {
            HealthUpdateEvent = new DamageDealEvent();
        }

        if (!healthBar && displayHealthBar)
        {
            SpawnHealthBarAbove();
        }

        SetHeatlhBar(actualHealth);
    }

    private void FixedUpdate()
    {
        // SetHeatlhBar(actualHealth);
    }

    public void DealDamage(GameObject attacker, int damage)
    {
        Debug.Log($"Deal Damage {gameObject.name}: deal dmg {damage}, heatlh: {actualHealth}");
        if (actualHealth > 0)
        {
            actualHealth -= damage;
            if (displayHealthBar)
            {
                // Debug.Log($"Heath: new health invoke {actualHealth}");
                SetHeatlhBar(actualHealth);
                HealthUpdateEvent.Invoke(gameObject, attacker, actualHealth);
                SpawnDamageTextAbove(damage);
            }

            if (actualHealth <= 0)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        HealthUpdateEvent.RemoveAllListeners();

        if (gameObject.CompareTag(Constants.PLAYER_TAG))
        {
            // TODO: game over
            Debug.Log($"{gameObject.name}: Game Over");
            DisplayHeroDiedMenu();
        } else if (gameObject.CompareTag(Constants.ENEMY_TAG))
        {
            Debug.Log($"{gameObject.name}: Enemy Died");
            Destroy(gameObject);
        } else
        {
            Debug.Log($"{gameObject.name}: Something Died");
            Destroy(gameObject);
        }
    }

    private void DisplayHeroDiedMenu()
    {
        Object menuPrefab = Resources.Load("UI/HeroDiedMenu");
        Instantiate(menuPrefab);
    }

    private void SpawnHealthBarAbove()
    {
        Object healthPrefab = Resources.Load("Prefabs/HealthBarAbove");
        healthBar = Instantiate(healthPrefab, transform.position + healtBarOffset, Quaternion.identity) as GameObject;

        healthBar.transform.parent = gameObject.transform;
        healthBar.GetComponent<IHealthBarController>().SetColor(color);
    }

    private void SetHeatlhBar(int newActualHealth)
    {
        if (healthBar)
        {
            float barSize = Mathf.Max((float)newActualHealth / maxHealth, 0f);
            healthBar.GetComponent<IHealthBarController>().SetSize(barSize);
            // Debug.Log($"SetHeatlhBar: healthbar");
            healthBar.GetComponent<IHealthBarController>().SetText($"{newActualHealth}/{maxHealth}");
        }
    }

    private void SpawnDamageTextAbove(int damage)
    {
        Object damagePopup = Resources.Load("UI/DamagePopup");
        GameObject popup = Instantiate(damagePopup, transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(0.8f, 1.5f), 0), Quaternion.identity) as GameObject;

        popup.GetComponent<Text>().text = $"{damage}";
        popup.transform.SetParent(GameObject.Find("WorldCanvas").transform);
    }

    public void Upgrade(int value)
    {
        if (!gameObject.CompareTag(Constants.PLAYER_TAG))
            return;

        actualHealth += value;
        maxHealth += value;
        // Debug.Log($"Upgrade: healthbar");
        SetHeatlhBar(actualHealth);
    } 
}
