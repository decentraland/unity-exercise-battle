using UnityEngine;


public sealed class BattleInstantiator : MonoBehaviour
{
    public static BattleInstantiator instance { get; private set; }

    [SerializeField] private ArmyModelSO army1Model;

    [SerializeField] private ArmyModelSO army2Model;

    [SerializeField] private Warrior warriorPrefab;

    [SerializeField] private Archer archerPrefab;

    [SerializeField] private BoxCollider leftArmySpawnBounds;

    [SerializeField] private BoxCollider rightArmySpawnBounds;

    public readonly Army Army1 = new();
    public readonly Army Army2 = new();

    public ArcherArrow arrowPrefab;

    public Color army1Color;
    public Color army2Color;

    public GameOverMenu gameOverMenu;

    private GameObjectPool pool;
    private Transform cameraTransform;

    private void InstanceArmy(IArmyModel model, Army army, Bounds instanceBounds)
    {
        for (var i = 0; i < model.warriors; i++)
        {
            var go = Instantiate(warriorPrefab.gameObject);

            go.transform.position = Utils.GetRandomPositionInBounds(instanceBounds);
            go.GetComponentInChildren<Warrior>().army = army;
            go.GetComponentInChildren<Warrior>().armyModel = model;
            go.GetComponentInChildren<Renderer>().material.color = army.color;

            army.Add(go.GetComponent<Warrior>());
        }

        for (var i = 0; i < model.archers; i++)
        {
            var go = Object.Instantiate(archerPrefab.gameObject);
            go.transform.position = Utils.GetRandomPositionInBounds(instanceBounds);

            go.GetComponentInChildren<Archer>().army = army;
            go.GetComponentInChildren<Archer>().armyModel = model;
            go.GetComponentInChildren<Renderer>().material.color = army.color;

            army.Add(go.GetComponent<Archer>());
        }
    }

    private void Awake()
    {
        instance = this;

        Army1.color = army1Color;
        Army1.enemyArmy = Army2;

        Army2.color = army2Color;
        Army2.enemyArmy = Army1;

        InstanceArmy(army1Model, Army1, leftArmySpawnBounds.bounds);
        InstanceArmy(army2Model, Army2, rightArmySpawnBounds.bounds);

        pool = new GameObjectPool(arrowPrefab.gameObject, 20, 100);
        Army1.Pool = pool;
        Army2.Pool = pool;

        cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        if (Army1.Units.Count == 0 || Army2.Units.Count == 0)
        {
            gameOverMenu.gameObject.SetActive(true);
            gameOverMenu.Populate();
        }

        Army1.center = Utils.GetCenter(Army1.Units);
        Army2.center = Utils.GetCenter(Army2.Units);

        var mainCenter = Army1.center + Army2.center;
        mainCenter *= 0.5f;

        forwardTarget = (mainCenter - cameraTransform.position).normalized;

        cameraTransform.forward += (forwardTarget - cameraTransform.forward) * 0.1f;
    }

    private Vector3 forwardTarget;
}