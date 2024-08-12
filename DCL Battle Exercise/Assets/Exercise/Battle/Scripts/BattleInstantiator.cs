using System.Collections.Generic;
using Exercise.LaunchMenu.Scripts;
using Exercise.Models.Scripts;
using UnityEngine;

namespace Exercise.Battle.Scripts
{
    public sealed class BattleInstantiator : MonoBehaviour
    {
        public static BattleInstantiator instance { get; private set; }

        [SerializeField] private ArmyModelSO[] armyModels;
        [SerializeField] private BoxCollider[] armySpawnBounds;

        [SerializeField] private Warrior warriorPrefab;

        [SerializeField] private Archer archerPrefab;

        public readonly List<Army> Armies = new();

        public ArcherArrow arrowPrefab;
        public GameOverMenu gameOverMenu;

        private GameObjectPool pool;
        private Transform cameraTransform;

        private void InstanceArmy(IArmyModel model, Army army, Bounds instanceBounds)
        {
            for (var i = 0; i < model.warriors; i++)
            {
                var go = Instantiate(warriorPrefab.gameObject);

                go.transform.position = Utils.GetRandomPositionInBounds(instanceBounds);
                go.GetComponentInChildren<Warrior>().Setup(army, model);
                go.GetComponentInChildren<Renderer>().material.color = army.Color;

                army.Add(go.GetComponent<Warrior>());
            }

            for (var i = 0; i < model.archers; i++)
            {
                var go = Object.Instantiate(archerPrefab.gameObject);
                go.transform.position = Utils.GetRandomPositionInBounds(instanceBounds);
                go.GetComponentInChildren<Archer>().Setup(army, model);
                go.GetComponentInChildren<Renderer>().material.color = army.Color;

                army.Add(go.GetComponent<Archer>());
            }
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
                        //TODO combine enemies
                        army.EnemyArmy = potentialArmy;
                }
            }

            cameraTransform = Camera.main.transform;
        }

        private void Update()
        {
            var mainCenter = Vector3.zero;
            foreach (var army in Armies)
            {
                if (army.Units.Count == 0)
                {
                    gameOverMenu.gameObject.SetActive(true);
                    gameOverMenu.Populate();
                }
                else
                {
                    army.Center = Utils.GetCenter(army.Transforms);
                }

                mainCenter += army.Center;
            }

            mainCenter *= 0.5f;

            forwardTarget = (mainCenter - cameraTransform.position).normalized;

            cameraTransform.forward += (forwardTarget - cameraTransform.forward) * 0.1f;
        }

        private Vector3 forwardTarget;
    }
}