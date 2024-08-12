using System;
using Exercise.Models.Scripts;
using UnityEngine;

namespace Exercise.Battle.Scripts
{
    [RequireComponent(typeof(Damage))]
    public sealed class Warrior : UnitBase
    {
        public void Setup(Army army, IArmyModel armyModel)
        {
            Army = army;
            ArmyModel = armyModel;
            switch (ArmyModel.strategy)
            {
                case ArmyStrategy.Basic:
                    gameObject.AddComponent<WarriorAttackBehaviorBasic>();
                    break;
                case ArmyStrategy.Defensive:
                    gameObject.AddComponent<WarriorAttackBehaviorDefensive>();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _attackBehavior = GetComponent<AttackBehavior>();
            _attackBehavior.Army = army;
        }
    }
}