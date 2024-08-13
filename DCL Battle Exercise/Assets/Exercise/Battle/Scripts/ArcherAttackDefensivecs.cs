using System.Collections.Generic;
using UnityEngine;

namespace Exercise.Battle.Scripts
{
    public sealed class ArcherAttackBehaviorDefensive : ArcherAttackBehavior
    {
        public override void UpdateStrategy(Vector3 toMove)
        {
            base.UpdateStrategy(toMove);

            var enemyCenter = _army.EnemyArmy.Center;
            var distToEnemyX = Mathf.Abs(enemyCenter.x - _transform.position.x);

            if (distToEnemyX > _attack.Range && CanMove())
            {
                if (enemyCenter.x < _transform.position.x)
                {
                    _movement.Move(Vector3.left);
                }

                if (enemyCenter.x > _transform.position.x)
                    _movement.Move(Vector3.right);
            }

            var enemies = _army.EnemyArmy.Transforms;
            var distToNearest = Utils.GetNearestObject(gameObject, enemies, out var nearestEnemy);

            if (nearestEnemy == null)
                return;

            if (distToNearest < _attack.Range)
            {
                var toNearest = (nearestEnemy.transform.position - _transform.position).normalized;
                toNearest.Scale(new Vector3(1, 0, 1));

                var flank = Quaternion.Euler(0, 90, 0) * toNearest;
                if (CanMove())
                    _movement.Move(-(toNearest + flank).normalized);
            }
            else
            {
                var toNearest = (nearestEnemy.transform.position - _transform.position).normalized;
                toNearest.Scale(new Vector3(1, 0, 1));
                if (CanMove())
                    _movement.Move(toNearest.normalized);
            }

            Attack(nearestEnemy);
        }
    }
}