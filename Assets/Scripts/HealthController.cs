using UnityEngine;

public class HealthController : MonoBehaviour
{
    public int maxHealth = 100;
    public int actualHealth = 100;
    public Color color = Color.green;
    public Vector3 healtBarOffset = new Vector3(0, 1.5f);
    public HealthBarController healthBar;

    private void Start()
    {
        if (!healthBar)
        {
            SpawnHealthBar();
        }
    }

    public void DealDamage(int damage)
    {
        Debug.Log(gameObject.name + ": deal dmg " + damage + ", heatlh: " + actualHealth);
        if (actualHealth > 0)
        {
            actualHealth -= damage;
            SetHeatlhBar(actualHealth);

            if (actualHealth <= 0)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        Debug.Log(gameObject.name + ": Died");
        Destroy(gameObject);
    }

    private void SpawnHealthBar()
    {
        Object healthPrefab = Resources.Load("Prefabs/HealthBar");
        GameObject healthBarObject = Instantiate(healthPrefab, transform.position + healtBarOffset, Quaternion.identity) as GameObject;
        // GameObject healthBarObject = transform.Find("HealthBar").gameObject;

        healthBarObject.transform.parent = gameObject.transform;
        healthBar = healthBarObject.GetComponent<HealthBarController>();
        healthBar.SetColor(color);
    }

    private void SetHeatlhBar(int newActualHealth)
    {
        if (healthBar)
        {
            float barSize = Mathf.Max((float)newActualHealth / maxHealth, 0f);
            healthBar.SetSize(barSize);
            Debug.Log("Bar size: " + barSize);
        }
    }
}
