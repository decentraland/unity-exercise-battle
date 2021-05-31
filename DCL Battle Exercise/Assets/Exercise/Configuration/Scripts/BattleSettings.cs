using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create BattleSettings", fileName = "BattleSettings", order = 0)]
public class BattleSettings : ScriptableObject
{
    public enum ArmyStrategy
    {
        None,
        StrategyDefensive,
        StrategyBasic
    }

    [System.Serializable]
    public class ArmySettings
    {
        public int warriorCount;
        public int archerCount;
        public ArmyStrategy armyStrategy;
    }

    public ArmySettings army1Settings;
    public ArmySettings army2Settings;
}