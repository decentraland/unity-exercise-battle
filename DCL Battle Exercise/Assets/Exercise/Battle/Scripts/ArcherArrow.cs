using System;
using UnityEngine;

public sealed class ArcherArrow : MonoBehaviour
{
    public float speed;

    [NonSerialized] public Vector3 target;
    [NonSerialized] public float attack;

    public Army army;

    public void Update()
    {
        var direction = (target - transform.position).normalized;
        transform.position += direction * speed;
        transform.forward = direction;

        foreach (var unitGo in army.enemyArmy.Units)
        {
            var dist = Vector3.Distance(unitGo.transform.position, transform.position);

            if (!(dist < speed)) continue;
            var unit = unitGo.GetComponent<UnitBase>();
            unit.Hit(gameObject);
            Destroy(gameObject);
            return;
        }

        if (Vector3.Distance(transform.position, target) < speed)
        {
            Destroy(gameObject);
        }
    }
}