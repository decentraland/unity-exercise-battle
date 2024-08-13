using UnityEngine;

namespace Exercise.Models.Scripts
{
    public interface IArmyModel
    {
        string name { get; }
        Color color { get; }
        ArmyDivision[] divisions { get; }
        ArmyStrategy strategy { get; set; }
    }

    public enum ArmyStrategy
    {
        Basic = 0,
        Defensive = 1
    }
}