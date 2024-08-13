using UnityEngine;

namespace Exercise.Battle.Scripts
{
    [RequireComponent(typeof(Damage))]
    public sealed class Warrior : UnitBase
    {
        public void Setup(Army army)
        {
            _army = army;
            _attackBehavior = GetComponent<AttackBehavior>();
        }
    }
}