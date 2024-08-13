using System;
using System.Collections.Generic;
using Exercise.LaunchMenu.Scripts;
using Exercise.Models.Scripts;
using UnityEngine;

namespace Exercise.Battle.Scripts
{
    public sealed class BattleInstantiator : MonoBehaviour
    {
        public static BattleInstantiator instance { get; private set; }

        [SerializeField] private ArmyModelSo[] armyModels;
        [SerializeField] private BoxCollider[] armySpawnBounds;

        public readonly List<Army> Armies = new();

        public ArcherArrow arrowPrefab;
        public GameOverMenu gameOverMenu;

        private GameObjectPool pool;
        private Transform cameraTransform;

        private static void InstanceArmy(IArmyModel model, Army army, Bounds instanceBounds)
        {
            foreach (var division in model.divisions)
            {
                for (var i = 0; i < division.count; i++)
                {
                    var go = Instantiate(division.unitPrefab.gameObject);
                    go.transform.position = Utils.GetRandomPositionInBounds(instanceBounds);
                    SetupUnit(division.type, go.GetComponentInChildren<Unit>(), army, model);
                    go.GetComponentInChildren<Renderer>().material.color = army.Color;
                    army.Add(go.GetComponent<Unit>());
                }
            }
        }

        private static Unit SetupUnit(UnitType type, Unit unit, Army army, IArmyModel armyModel)
        {
            switch (type)
            {
                case UnitType.Warrior:
                    switch (armyModel.strategy)
                    {
                        case ArmyStrategy.Basic:
                            unit.gameObject.AddComponent<WarriorAttackBehaviorBasic>();
                            break;
                        case ArmyStrategy.Defensive:
                            unit.gameObject.AddComponent<WarriorAttackBehaviorDefensive>();
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    break;

                case UnitType.Archer:
                    switch (armyModel.strategy)
                    {
                        case ArmyStrategy.Basic:
                            unit.gameObject.AddComponent<ArcherAttackBehaviorBasic>();
                            break;
                        case ArmyStrategy.Defensive:
                            unit.gameObject.AddComponent<ArcherAttackBehaviorDefensive>();
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            unit.Setup(army);
            return unit;
        }

        private void Awake()
        {
            instance = this;

            pool = new GameObjectPool(arrowPrefab.gameObject, 20, 100);
            for (var index = 0; index < armyModels.Length; index++)
            {
                var armyModel = armyModels[index];
                var army = new Army
                {
                    Name = armyModel.name,
                    Color = armyModel.color,
                    Pool = pool
                };
                var armySpawnBound = index < armySpawnBounds.Length ? armySpawnBounds[index] : new BoxCollider();
                InstanceArmy(armyModel, army, armySpawnBound.bounds);
                Armies.Add(army);
            }

            foreach (var army in Armies)
            {
                foreach (var potentialArmy in Armies)
                {
                    if (army != potentialArmy)
                        army.EnemyArmy += potentialArmy;
                }
            }

            cameraTransform = Camera.main.transform;
        }

        private void Update()
        {
            var mainCenter = Vector3.zero;

            foreach (var army in Armies)
            {
                if (army.IsDefeated)
                {
                    gameOverMenu.gameObject.SetActive(true);
                    gameOverMenu.Populate(army, Armies);
                }
                else
                {
                    army.Center = Utils.GetCenter(army.Transforms);
                    mainCenter += army.Center;
                }
            }

            mainCenter *= 0.5f;

            forwardTarget = (mainCenter - cameraTransform.position).normalized;

            cameraTransform.forward += (forwardTarget - cameraTransform.forward) * 0.1f;
        }

        private Vector3 forwardTarget;
    }
}