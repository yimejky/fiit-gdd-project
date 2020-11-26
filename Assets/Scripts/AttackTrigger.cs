using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTrigger : MonoBehaviour
{
    public Weapon weapon;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        weapon.Attack();
    }
}
