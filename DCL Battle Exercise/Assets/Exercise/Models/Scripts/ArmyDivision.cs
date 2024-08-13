using System;
using Exercise.Battle.Scripts;
using UnityEngine;

namespace Exercise.Models.Scripts
{
    [Serializable]
    public sealed class ArmyDivision
    {
        [SerializeField] private int countValue;

        public int count
        {
            get => countValue;
            set => countValue = value;
        }

        [SerializeField] private UnitType typeValue = UnitType.Warrior;

        public UnitType type
        {
            get => typeValue;
            set => typeValue = value;
        }

        [SerializeField] private Unit unitPrefabValue;

        public Unit unitPrefab
        {
            get => unitPrefabValue;
            set => unitPrefabValue = value;
        }
    };

    public enum UnitType
    {
        Warrior = 0,
        Archer = 1
    }
}