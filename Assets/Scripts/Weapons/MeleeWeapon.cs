using System;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    public LayerMask hurtboxLayer;
    public Transform attackPoint;
    public float attackRange = 0.8f;

    private float animationCooldown;
    private Vector2 defaultAttackPosition;
    private Vector2 defaultPosition;
    private Transform body;
    private Animator weaponAnimator;
    private Quaternion defaultRotation;
    private GameObject wielderGameObject;
    private readonly float defaultAnimationCooldown = 0.42f;
    private readonly float swordAnimationOffset = 1.5f;

    private void Awake()
    {
        body = transform.Find("Body");
        weaponAnimator = body.GetComponent<Animator>();
        defaultPosition = body.transform.localPosition;
        defaultRotation = body.transform.localRotation;
        defaultAttackPosition = attackPoint.localPosition;
    }
    private new void Update()
    {
        base.Update();
        wielderGameObject = transform.parent.gameObject;

        animationCooldown -= Time.deltaTime;
        if (animationCooldown >= 0)
        {
            animationCooldown -= Time.deltaTime;
        }
        else
        { 
            body.localPosition = defaultPosition;
            body.localRotation = defaultRotation;
        }

        attackPoint.localPosition = CalculateAttackPoint();
    }
    public override void Attack()
    {
        if (isOnCooldown())
            return;

        Vector3 attackPosition = attackPoint.position;
        // Debug.Log($"Attack {attackPoint.localPosition}");
        Collider2D[] hurtboxes = Physics2D.OverlapCircleAll(attackPosition, attackRange, hurtboxLayer);
        // enemies hurtbox
        foreach (Collider2D hurtbox in hurtboxes)
        {
            GameObject hurtboxParent = hurtbox.transform.parent.gameObject;
            bool isDestroy = hurtboxParent.CompareTag(Constants.DESTROYABLE_TAG);
            bool ignoreHimself = hurtboxParent == transform.parent.gameObject;
            if (ignoreHimself || isDestroy)
                continue;

            // enemy cant hit another enemy
            if (wielderGameObject.CompareTag(Constants.ENEMY_TAG) && hurtboxParent.CompareTag(Constants.ENEMY_TAG))
                continue;

            int range = (int)(damage * weaponConfig.damageRandomRange);
            int finalDamage = UnityEngine.Random.Range(damage - range, damage + range);
            // Debug.Log($"Hit from {transform.parent.name} to {hurtboxParent.name}");

            hurtboxParent.GetComponent<HealthController>().DealDamage(wielderGameObject, finalDamage);
            hurtboxParent.GetComponent<KnockbackController>().Knock(gameObject.transform.position, weaponConfig.knockbackPower, weaponConfig.knockbackTime);

            // if player hit from above, knock him some distance
            if (wielderGameObject.CompareTag(Constants.PLAYER_TAG) && attackPoint.localPosition.y < 0)
            {
                transform.parent.GetComponent<KnockbackController>().Knock(hurtboxParent.transform.position, weaponConfig.knockbackPower, weaponConfig.knockbackTime);
            }

            break;
        }

        // destroyable hurtbox
        foreach (Collider2D hurtbox in hurtboxes)
        {
            GameObject parent = hurtbox.transform.parent.gameObject;
            bool isDestroy = parent.CompareTag(Constants.DESTROYABLE_TAG);
            if (!isDestroy)
                continue;

            // Debug.Log($"Destroyable {parent.name}");
            parent.GetComponent<HealthController>().DealDamage(wielderGameObject, damage);
        }

        WeaponAnimation();
        wielderGameObject.GetComponent<AudioController>().PlayHit();
        base.Attack();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 attackPosition = attackPoint.position;
        Gizmos.DrawWireSphere(attackPosition, attackRange);
    }

    private void WeaponAnimation()
    {
        float angle;
        try
        {
            Vector2 swordDirection = wielderGameObject.GetComponent<IMeleeWeaponWielder>().GetMeeleAttackDirection();
            body.localPosition = attackPoint.transform.localPosition;
            angle = 90 * swordDirection.y / (swordDirection.x + swordAnimationOffset);
        }
        catch
        {
            angle = 0;
        }

        body.Rotate(new Vector3(0, 0, angle));
        weaponAnimator.Play("Attack");
        animationCooldown = defaultAnimationCooldown;
    }

    private Vector2 CalculateAttackPoint()
    {
        Vector2 swordDirection = wielderGameObject.GetComponent<IMeleeWeaponWielder>().GetMeeleAttackDirection();
        if (Math.Abs(swordDirection.y) > 0)
        {
            Vector2 vec = new Vector2(swordDirection.x * defaultAttackPosition.x, swordDirection.y);
            return vec;
        }

        return defaultAttackPosition;
    }
}
