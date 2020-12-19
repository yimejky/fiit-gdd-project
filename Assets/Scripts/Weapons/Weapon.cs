using UnityEngine;

public abstract class Weapon : MonoBehaviour, IUpgradable
{
    public string configName;

    protected int damage;
    protected WeaponConfig weaponConfig => GameConfigManager.Get().GetConfig<WeaponConfig>(configName);
    private float currentCooldown = 0;

    public virtual void Start()
    {
        damage = weaponConfig.defaultDamage;
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
        currentCooldown -= Time.deltaTime;
    }

    public void Upgrade(int value)
    {
        damage += value;
    }
}
