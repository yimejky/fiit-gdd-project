﻿using UnityEngine;

public class MeleeEnemy : Enemy, MeeleeWeaponWielder
{
    private float attackRange = 1.5f;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (knockbackController.canMove && playerDistance <= startAttackDistance)
        {
            if (playerDistance > attackRange)
            {
                // Debug.Log($"Debug distance {playerDistance}, {attackRange}");
                MoveToTarget(player);
            } else
            {
                weapon.Attack();
            }
        }
    }

    public Vector2 GetAttackDirection()
    {
        return new Vector2(1, 0);
    }
}