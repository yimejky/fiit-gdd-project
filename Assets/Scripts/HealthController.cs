using UnityEngine;

public class HealthController : MonoBehaviour
{
    public int maxHealth = 100;
    public int actualHealth = 100;
    public Vector3 healtBarOffset = new Vector3(0, 1.5f);
    private HealthBarController healthBar;

    private void Start()
    {
        Object healthPrefab = Resources.Load("Prefabs/HealthBar");
        GameObject healthBarObject = Instantiate(healthPrefab, Vector3.zero + healtBarOffset, Quaternion.identity) as GameObject;
        // GameObject healthBarObject = transform.Find("HealthBar").gameObject;

        healthBarObject.transform.parent = gameObject.transform;
        healthBar = healthBarObject.GetComponent<HealthBarController>();
        Debug.Log(healthBar);
    }

    public void DealDamage(int damage)
    {
        Debug.Log(gameObject.name + ": deal dmg " + damage + ", heatlh: " + actualHealth);
        if (actualHealth > 0)
        {
            actualHealth -= damage;
            float barSize = Mathf.Max((float) actualHealth / maxHealth, 0);
            Debug.Log("Bar size: " + barSize);
            healthBar.SetSize(barSize);

            if (actualHealth < 0)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        Debug.Log(gameObject.name + ": Died");
    }
}
