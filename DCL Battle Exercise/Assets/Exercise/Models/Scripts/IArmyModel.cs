public interface IArmyModel
{
    int warriors { get; set; }
    int archers { get; set; }
    ArmyStrategy strategy { get; set; }
}

public enum ArmyStrategy
{
    Basic = 0,
    Defensive = 1
}