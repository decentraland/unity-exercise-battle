using System;
using Exercise.Models.Scripts;

namespace Exercise.Battle.Scripts
{
    public sealed class Archer : UnitBase
    {
        public void Setup(Army army, IArmyModel armyModel)
        {
            Army = army;
            ArmyModel = armyModel;
            switch (ArmyModel.strategy)
            {
                case ArmyStrategy.Basic:
                    gameObject.AddComponent<ArcherAttackBehaviorBasic>().Army = Army;
                    break;
                case ArmyStrategy.Defensive:
                    gameObject.AddComponent<ArcherAttackBehaviorDefensive>().Army = Army;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _attackBehavior = GetComponent<AttackBehavior>();
            _attackBehavior.Army = army;
        }
    }
}