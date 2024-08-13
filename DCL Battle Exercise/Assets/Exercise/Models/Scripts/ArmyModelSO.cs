using System;
using UnityEngine;

namespace Exercise.Models.Scripts
{
    /// <summary>
    /// ScriptableObject containing the data of an army
    /// </summary>
    [CreateAssetMenu(menuName = "Create ArmyModel", fileName = "ArmyModel", order = 0)]
    public sealed class ArmyModelSo : ScriptableObject, IArmyModel
    {
        [SerializeField] private string nameValue;

        public string name
        {
            get => nameValue;
        }

        [SerializeField] private Color colorValue;

        public Color color
        {
            get => colorValue;
        }

        [SerializeField] private ArmyDivision[] divisionsValue = Array.Empty<ArmyDivision>();

        public ArmyDivision[] divisions
        {
            get => divisionsValue;
            set => divisionsValue = value;
        }

        [SerializeField] private ArmyStrategy strategyValue = ArmyStrategy.Basic;

        public ArmyStrategy strategy
        {
            get => strategyValue;
            set => strategyValue = value;
        }
    }
}