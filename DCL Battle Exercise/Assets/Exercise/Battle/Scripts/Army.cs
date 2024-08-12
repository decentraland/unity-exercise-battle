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
        public List<GameObject> Units { get; } = new();
        public List<Transform> Transforms { get; } = new();

        private List<Archer> _archers { get; } = new();
        private List<Warrior> _warriors { get; } = new();

        public void Add(Warrior warrior)
        {
            _warriors.Add(warrior);
            Units.Add(warrior.gameObject);
            Transforms.Add(warrior.transform);
        }

        public void Remove(Warrior warrior)
        {
            _warriors.Remove(warrior);
            Units.Remove(warrior.gameObject);
            Transforms.Remove(warrior.transform);
        }

        public void Add(Archer archer)
        {
            _archers.Add(archer);
            Units.Add(archer.gameObject);
            Transforms.Add(archer.transform);
        }

        public void Remove(Archer archer)
        {
            _archers.Remove(archer);
            Units.Remove(archer.gameObject);
            Transforms.Remove(archer.transform);
        }
    }
}