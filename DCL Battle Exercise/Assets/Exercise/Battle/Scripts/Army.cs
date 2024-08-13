using System;
using System.Collections.Generic;
using UnityEngine;

namespace Exercise.Battle.Scripts
{
    public sealed class Army
    {
        public Action<Unit> OnRemoved;
        public string Name;
        public Army EnemyArmy;
        public Color Color;
        public Vector3 Center = Vector3.zero;
        public GameObjectPool Pool;
        public List<Transform> Transforms { get; } = new();
        public bool IsDefeated => Transforms.Count == 0;
        private List<Unit> Units { get; } = new();

        public void Add(Unit unit)
        {
            Transforms.Add(unit.transform);
            Units.Add(unit);
        }

        public void Remove(Unit unit)
        {
            Transforms.Remove(unit.transform);
            Units.Remove(unit);
            OnRemoved?.Invoke(unit);
        }

        public static Army operator +(Army a, Army b)
        {
            if (b == null) return a;
            if (a == null) return b;

            var army = new Army
            {
                Name = a.Name + "+" + b.Name,
                Color = a.Color + b.Color,
                Center = a.Center + b.Center,
                Pool = a.Pool
            };
            
            a.OnRemoved += unit => army.Remove(unit);
            b.OnRemoved += unit => army.Remove(unit); 

            foreach (var unit in a.Units)
            {
                army.Add(unit);
            }

            foreach (var unit in b.Units)
            {
                army.Add(unit);
            }

            return army;
        }
    }
}