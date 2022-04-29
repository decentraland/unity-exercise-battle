using UnityEngine;


/// <summary>
/// ScriptableObject containing the data of an army
/// for simplicity's sake the use-case of updating the SO manually has been discarded, and
/// therefore the usage of ReadOnlyAttribute
/// </summary>
[CreateAssetMenu(menuName = "Create ArmyModel", fileName = "ArmyModel", order = 0)]
public class ArmyModelSO : ScriptableObject, IArmyModel
{
    [ReadOnlyAttribute, SerializeField] private int warriorsValue = 100;
    public int warriors
    {
        get => warriorsValue;
        set => warriorsValue = value;
    }

    [ReadOnlyAttribute, SerializeField] private int archersValue = 100;
    public int archers
    {
        get => archersValue;
        set => archersValue = value;
    }

    [ReadOnlyAttribute, SerializeField] private ArmyStrategy strategyValue = ArmyStrategy.Basic;
    public ArmyStrategy strategy
    {
        get => strategyValue;
        set => strategyValue = value;
    }
}