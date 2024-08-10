using System.Collections.Generic;
using UnityEngine;

public sealed class Army
{
    public Army enemyArmy;
    public Color color;
    public Vector3 center = Vector3.zero;
    public GameObjectPool Pool;
    public List<GameObject> Units { get; } = new();
    private List<Archer> Archers { get; } = new();
    private List<Warrior> Warriors { get; } = new();

    public void Add(Warrior warrior)
    {
        Warriors.Add(warrior);
        Units.Add(warrior.gameObject);
    }

    public void Remove(Warrior warrior)
    {
        Warriors.Remove(warrior);
        Units.Remove(warrior.gameObject);
    }

    public void Add(Archer archer)
    {
        Archers.Add(archer);
        Units.Add(archer.gameObject);
    }

    public void Remove(Archer archer)
    {
        Archers.Remove(archer);
        Units.Remove(archer.gameObject);
    }
}