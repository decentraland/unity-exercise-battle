using System.Collections.Generic;
using UnityEngine;

namespace Exercise.Battle.Scripts
{
    public sealed class WarriorAttackBehaviorBasic : WarriorAttackBehavior
    {
        public override void UpdateStrategy(List<GameObject> enemies, Vector3 toMove)
        {
            base.UpdateStrategy(enemies, toMove);

            _transform.position += toMove;
            Utils.GetNearestObject(gameObject, enemies, out var nearestEnemy);

            if (nearestEnemy == null)
                return;

            var toNearest = (nearestEnemy.transform.position - _transform.position).normalized;
            toNearest.Scale(new Vector3(1, 0, 1));
            if (CanMove())
                _movement.Move(toNearest.normalized);
            Attack(nearestEnemy);
        }
    }
}