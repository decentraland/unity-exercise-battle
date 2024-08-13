using System.Collections.Generic;
using UnityEngine;

namespace Exercise.Battle.Scripts
{
    public sealed class Army
    {
        public string Name;
        public Army EnemyArmy;
        public Color Color;
        public Vector3 Center = Vector3.zero;
        public GameObjectPool Pool;
        public List<Transform> Transforms { get; } = new();
        public bool IsDefeated => Transforms.Count == 0;

        public void Add(Warrior warrior) => Transforms.Add(warrior.transform);


        public void Remove(Warrior warrior) => Transforms.Remove(warrior.transform);

        public void Add(Archer archer) => Transforms.Add(archer.transform);

        public void Remove(Archer archer) => Transforms.Remove(archer.transform);
    }
}