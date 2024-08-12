using Exercise.Utils;
using UnityEngine;

namespace Exercise.Models.Scripts
{
    /// <summary>
    /// ScriptableObject containing the data of an army
    /// for simplicity's sake the use-case of updating the SO manually has been discarded, and
    /// therefore the usage of ReadOnlyAttribute
    /// </summary>
    [CreateAssetMenu(menuName = "Create ArmyModel", fileName = "ArmyModel", order = 0)]
    public sealed class ArmyModelSO : ScriptableObject, IArmyModel
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

        [ReadOnly, SerializeField] private int warriorsValue = 100;

        public int warriors
        {
            get => warriorsValue;
            set => warriorsValue = value;
        }

        [ReadOnly, SerializeField] private int archersValue = 100;

        public int archers
        {
            get => archersValue;
            set => archersValue = value;
        }

        [ReadOnly, SerializeField] private ArmyStrategy strategyValue = ArmyStrategy.Basic;

        public ArmyStrategy strategy
        {
            get => strategyValue;
            set => strategyValue = value;
        }
    }
}