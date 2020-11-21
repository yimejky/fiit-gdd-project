using JetBrains.Annotations;
using UnityEngine;

// inspired by https://www.youtube.com/watch?v=sPiVz1k-fEs
public class CombatSystem : MonoBehaviour
{
    public Weapon weapon;

    void Update()
    {
        if (Input.GetButtonDown("Attack"))
        {
            weapon.Attack();
        }
    }
}
