using System;
using UnityEngine;

namespace Exercise.Battle.Scripts
{
    public sealed class ArcherArrow : MonoBehaviour
    {
        public float speed;

        [NonSerialized] public Vector3 Target;
        [NonSerialized] public float Attack;
        public Army Army;

        public void Update()
        {
            var direction = (Target - transform.position).normalized;
            transform.position += direction * speed;
            transform.forward = direction;

            foreach (var unitGo in Army.EnemyArmy.Units)
            {
                var dist = Vector3.Distance(unitGo.transform.position, transform.position);

                if (!(dist < speed)) continue;
                var unit = unitGo.GetComponent<UnitBase>();
                unit.Hit(gameObject);
                Army.Pool.ReleaseObject(gameObject);
                return;
            }

            if (Vector3.Distance(transform.position, Target) < speed)
            {
                Destroy(gameObject);
            }
        }
    }
}