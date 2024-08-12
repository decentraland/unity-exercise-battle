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
            var arrowGo = Army.Pool.GetObject(transform.position);
            var archerArrow = arrowGo.GetComponent<ArcherArrow>();
            archerArrow.Target = enemy.transform.position;
            archerArrow.Army = Army;

            _animator?.SetTrigger(AnimatorAttackIndex);

            arrowGo.GetComponent<Renderer>().material.color = Army == BattleInstantiator.instance.Army1
                ? BattleInstantiator.instance.army1Color
                : BattleInstantiator.instance.army2Color;
        }
    }
}