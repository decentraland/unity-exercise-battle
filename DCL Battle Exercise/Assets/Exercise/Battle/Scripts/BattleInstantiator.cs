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

    public readonly Army army1 = new();
    public readonly Army army2 = new();

    public GameObjectPool pool;
    public ArcherArrow arrowPrefab;

    public Color army1Color;
    public Color army2Color;

    public GameOverMenu gameOverMenu;

    private Transform cameraTransform;

    void InstanceArmy(IArmyModel model, Army army, Bounds instanceBounds)
    {
        for (var i = 0; i < model.warriors; i++)
        {
            var go = Instantiate(warriorPrefab.gameObject);

            go.transform.position = Utils.GetRandomPosInBounds(instanceBounds);
            go.GetComponentInChildren<Warrior>().army = army;
            go.GetComponentInChildren<Warrior>().armyModel = model;
            go.GetComponentInChildren<Renderer>().material.color = army.color;

            army.Add(go.GetComponent<Warrior>());
        }

        for (var i = 0; i < model.archers; i++)
        {
            var go = Object.Instantiate(archerPrefab.gameObject);
            go.transform.position = Utils.GetRandomPosInBounds(instanceBounds);

            go.GetComponentInChildren<Archer>().army = army;
            go.GetComponentInChildren<Archer>().armyModel = model;
            go.GetComponentInChildren<Renderer>().material.color = army.color;

            army.Add(go.GetComponent<Archer>());
        }
    }

    void Awake()
    {
        instance = this;

        army1.color = army1Color;
        army1.enemyArmy = army2;

        army2.color = army2Color;
        army2.enemyArmy = army1;

        InstanceArmy(army1Model, army1, leftArmySpawnBounds.bounds);
        InstanceArmy(army2Model, army2, rightArmySpawnBounds.bounds);

        pool = new GameObjectPool(arrowPrefab.gameObject, 20, 100);
        army1.pool = pool;
        army2.pool = pool;

        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        if (army1.Units.Count == 0 || army2.Units.Count == 0)
        {
            gameOverMenu.gameObject.SetActive(true);
            gameOverMenu.Populate();
        }

        army1.center = Utils.GetCenter(army1.Units);
        army2.center = Utils.GetCenter(army2.Units);

        var mainCenter = army1.center + army2.center;
        mainCenter *= 0.5f;

        forwardTarget = (mainCenter - cameraTransform.position).normalized;

        cameraTransform.forward += (forwardTarget - cameraTransform.forward) * 0.1f;
    }

    private Vector3 forwardTarget;
}