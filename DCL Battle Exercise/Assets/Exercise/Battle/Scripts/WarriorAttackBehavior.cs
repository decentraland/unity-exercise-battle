using UnityEngine;

namespace Exercise.Battle.Scripts
{
    public abstract class WarriorAttackBehavior : AttackBehavior
    {
        protected void Attack(GameObject target)
        {
            if (_attackCooldown > 0)
                return;

            if (Vector3.Distance(_transform.position, target.transform.position) > _attack.Range)
                return;

            var targetUnit = target.GetComponentInChildren<Unit>();

            if (targetUnit == null)
                return;

            _attackCooldown = _attack.MaxCooldown;

            _animator?.SetTrigger(AnimatorAttackIndex);

            targetUnit.Hit(gameObject);
        }
    }
}