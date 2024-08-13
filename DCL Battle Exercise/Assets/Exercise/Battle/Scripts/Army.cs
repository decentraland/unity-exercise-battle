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

        public void Add(Unit unit) => Transforms.Add(unit.transform);
        public void Remove(Unit unit) => Transforms.Remove(unit.transform);
    }
}