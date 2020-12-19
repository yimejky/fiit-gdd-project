using UnityEngine;

public abstract class Weapon : MonoBehaviour, IUpgradable
{
    public string configName;
    protected int damage;
    protected WeaponConfig weaponConfig => GameConfigManager.Get().GetConfig<WeaponConfig>(configName);
    private float currentCooldown = 0;

    protected void Start()
    {
        // Debug.Log($"Weapon start {weaponConfig.defaultDamage}");
        damage = weaponConfig.defaultDamage;
        if (transform.parent.gameObject.name == "Hero")
        {
            if (this.GetType().Name == "MeleeWeapon")
            {
                // Debug.Log($"Upgrade sword {damage}");
                Upgrade(StatsUpgrades.Instance.GetStat("sword") * PlayerController.playerControllerConfig.meleeWeaponCoefficient);
            } else
            {
                // Debug.Log($"Upgrade bow {damage}");
                Upgrade(StatsUpgrades.Instance.GetStat("bow") * PlayerController.playerControllerConfig.rangedWeaponCoefficient);
            }
        }
    }

    public virtual void Attack()
    {
        currentCooldown = weaponConfig.attackCooldown;
    }

    protected bool isOnCooldown()
    {
        return currentCooldown > 0;
    }

    protected void Update()
    {
        if (currentCooldown >= 0)
        {
            currentCooldown -= Time.deltaTime;
        }
    }

    public void Upgrade(int value)
    {
        // Debug.Log($"before upgrade damage: {damage}, value {value}");
        damage += value;
        // Debug.Log($"After upgrade damage: {damage}, value {value}");
    }
}
