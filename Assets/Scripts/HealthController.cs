using UnityEngine;

public class HealthController : MonoBehaviour
{
    public Vector3 respawnPosition;
    public int maxHealth = 100;
    public int actualHealth = 100;
    public bool displayHealthBar = true;

    // healtbar settings
    public Color color = Color.green;
    public Vector3 healtBarOffset = new Vector3(0, 1.5f);
    public GameObject healthBar;

    private void Start()
    {
        respawnPosition = transform.position;
        if (!healthBar && displayHealthBar)
        {
            SpawnHealthBar();
        }
    }

    private void FixedUpdate()
    {
        SetHeatlhBar(actualHealth);
    }

    public void DealDamage(int damage)
    {
        // Debug.Log(gameObject.name + ": deal dmg " + damage + ", heatlh: " + actualHealth);
        if (actualHealth > 0)
        {
            actualHealth -= damage;
            if (displayHealthBar)
            {
                SetHeatlhBar(actualHealth);
            }

            if (actualHealth <= 0)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        if (gameObject.CompareTag(Constants.PLAYER_TAG))
        {
            // TODO: game over
            Debug.Log($"{gameObject.name}: Game Over");
            RespawnPlayer();
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

    private void RespawnPlayer()
    {
        transform.position = respawnPosition;
        actualHealth = maxHealth;
    }

    private void SpawnHealthBar()
    {
        Object healthPrefab = Resources.Load("Prefabs/HealthBarAbove");
        healthBar = Instantiate(healthPrefab, transform.position + healtBarOffset, Quaternion.identity) as GameObject;
        // GameObject healthBarObject = transform.Find("HealthBar").gameObject;

        healthBar.transform.parent = gameObject.transform;
        healthBar.GetComponent<IHealthBarController>().SetColor(color);
    }

    private void SetHeatlhBar(int newActualHealth)
    {
        if (healthBar)
        {
            float barSize = Mathf.Max((float)newActualHealth / maxHealth, 0f);
            healthBar.GetComponent<IHealthBarController>().SetSize(barSize);
        }
    }
}
