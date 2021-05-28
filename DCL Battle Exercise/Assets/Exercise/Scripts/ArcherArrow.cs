using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherArrow : MonoBehaviour
{
    public float speed;

    [NonSerialized] public Vector3 target;
    [NonSerialized] public float attack;

    public Army allyArmy;
    public Army enemyArmy;

    public void Update()
    {
        Vector3 direction = (target - transform.position).normalized;
        transform.position += direction * speed;
        transform.forward = direction;

        foreach ( var a in enemyArmy.GetUnits() )
        {
            float dist = Vector3.Distance(a.transform.position, transform.position);

            if (dist < 2.0f)
            {
                IUnit unit = a.GetComponent<IUnit>();
                unit.health -= attack - unit.defense;

                if ( unit.health < 0 )
                {
                    enemyArmy.warriors.Remove(a.GetComponent<Warrior>());
                    enemyArmy.archers.Remove(a.GetComponent<Archer>());
                    Destroy( a );
                }

                Destroy(gameObject);
                return;
            }

            if ( Vector3.Distance(transform.position, target) < 2.0f)
            {
                Destroy(gameObject);
                return;
            }
        }
    }
}