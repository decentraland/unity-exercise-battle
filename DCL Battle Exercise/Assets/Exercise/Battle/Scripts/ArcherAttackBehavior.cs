using UnityEngine;

namespace Exercise.Battle.Scripts
{
    public abstract class ArcherAttackBehavior : AttackBehavior
    {
        protected void Attack(GameObject enemy)
        {
            if (_attackCooldown > 0)
                return;

            if (Vector3.Distance(transform.position, enemy.transform.position) > _attack.Range)
                return;

            _attackCooldown = _attack.MaxCooldown;
            var arrowGo = _army.Pool.GetObject(transform.position);
            var archerArrow = arrowGo.GetComponent<ArcherArrow>();
            archerArrow.Target = enemy.transform.position;
            archerArrow.Army = _army;

            _animator?.SetTrigger(AnimatorAttackIndex);

            arrowGo.GetComponent<Renderer>().material.color = _army.Color;
        }
    }
}