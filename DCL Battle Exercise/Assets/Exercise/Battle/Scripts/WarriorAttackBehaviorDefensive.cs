using System.Collections.Generic;
using UnityEngine;

namespace Exercise.Battle.Scripts
{
    public sealed class WarriorAttackBehaviorDefensive : WarriorAttackBehavior
    {
        public override void UpdateStrategy(List<GameObject> enemies, Vector3 toMove)
        {
            base.UpdateStrategy(enemies, toMove);

            var enemyCenter = Army.EnemyArmy.Center;

            _transform.position += toMove;

            if (Mathf.Abs(enemyCenter.x - _transform.position.x) > 20)
            {
                if (enemyCenter.x < _transform.position.x && CanMove())
                    _movement.Move(Vector3.left);

                if (enemyCenter.x > _transform.position.x && CanMove())
                    _movement.Move(Vector3.right);
            }

            Utils.GetNearestObject(gameObject, enemies, out var nearestObject);

            if (nearestObject == null)
                return;

            if (CanMove())
            {
                if (_attackCooldown <= 0)
                    _movement.Move((nearestObject.transform.position - _transform.position).normalized);
                else
                {
                    _movement.Move((nearestObject.transform.position - _transform.position).normalized * -1);
                }
            }

            Attack(nearestObject);
        }
    }
}