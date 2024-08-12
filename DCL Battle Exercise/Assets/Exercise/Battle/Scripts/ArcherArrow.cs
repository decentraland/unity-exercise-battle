using System;
using UnityEngine;

namespace Exercise.Battle.Scripts
{
    [RequireComponent(typeof(Damage))]
    [RequireComponent(typeof(Movement))]
    public sealed class ArcherArrow : MonoBehaviour
    {
        [NonSerialized] public Vector3 Target;
        public Army Army;

        private Movement _movement;
        private Transform _transform;

        private void Awake()
        {
            _movement = GetComponent<Movement>();
            _transform = transform;
        }

        public void Update()
        {
            var direction = (Target - _transform.position).normalized;
            _movement.Move(direction);
            _transform.forward = direction;

            //TODO: move out
            foreach (var unitGo in Army.EnemyArmy.Units)
            {
                var dist = Vector3.Distance(unitGo.transform.position, _transform.position);
                if (!(dist < _movement.Speed)) continue;
                var unit = unitGo.GetComponent<UnitBase>();
                unit.Hit(gameObject);
                Army.Pool.ReleaseObject(gameObject);
                return;
            }

            if (Vector3.Distance(_transform.position, Target) < _movement.Speed)
            {
                Destroy(gameObject);
            }
        }
    }
}