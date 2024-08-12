using UnityEngine;

namespace Exercise.Models.Scripts
{
    public interface IArmyModel
    {
        string name { get; }
        Color color { get; }
        int warriors { get; set; }
        int archers { get; set; }
        ArmyStrategy strategy { get; set; }
    }

    public enum ArmyStrategy
    {
        Basic = 0,
        Defensive = 1
    }
}