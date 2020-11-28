using System;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    public LayerMask hurtboxLayer;
    public Transform attackPoint;
    public float attackRange = 0.8f;
    public int damage = 10;

    private Animator weaponAnimator;
	private Vector3 defaultAttackPosition;
    private Vector3 defaultPosition;
    private Quaternion defaultRotation;
    private float animationCooldown;
    private MeeleeWeaponWielder wielder;
    private Transform body;
    

    private void Start()
    {
        body = transform.Find("Body");
        weaponAnimator = body.GetComponent<Animator>();
        defaultPosition = body.transform.localPosition;
        defaultRotation = body.transform.localRotation;
        defaultAttackPosition = attackPoint.localPosition;
        wielder = transform.parent.GetComponent<MeeleeWeaponWielder>();
    }
    private new void Update()
    {
        base.Update();
        animationCooldown -= Time.deltaTime;
        if (animationCooldown < 0)
        {
            body.localPosition = defaultPosition;
            body.localRotation = defaultRotation;
        }
        attackPoint.localPosition = calculateAttackPoint();
    }
    public override void Attack()
    {
        if (isOnCooldown())
            return;

        Vector3 attackPosition = attackPoint.position;
        Collider2D[] hurtboxes = Physics2D.OverlapCircleAll(attackPosition, attackRange, hurtboxLayer);
        foreach (Collider2D hurtbox in hurtboxes)
        {
            GameObject parent = hurtbox.transform.parent.gameObject;
            if (parent == transform.parent.gameObject)
                continue;

            Debug.Log("Hit " + hurtbox.name);
            parent.GetComponent<HealthController>().DealDamage(damage);
            parent.GetComponent<KnockbackController>().Knock(gameObject, false);
            break;
        }

        WeaponAnimation();
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
            Vector2 swordDirection = wielder.GetAttackDirection();
            body.localPosition = attackPoint.transform.localPosition;
            angle = 90 * swordDirection.y / (swordDirection.x + 1);
        }
        catch
        {
            angle = 0;
        }

        body.Rotate(new Vector3(0, 0, angle));
        weaponAnimator.Play("Attack");
        animationCooldown = 0.42f;
    }

    private Vector2 calculateAttackPoint()
    {
        // Rotate around
        float centerX = 0.7f;
        float centerY = 0.488f;
        float r = Math.Abs(defaultAttackPosition.x - centerX);
        Vector2 swordDirection = wielder.GetAttackDirection();
        
        return new Vector2(centerX + r * swordDirection.x, centerY + r * swordDirection.y);
    }
}

public interface MeeleeWeaponWielder
{
    Vector2 GetAttackDirection();
}
