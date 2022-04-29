using System.Collections;
using UnityEditor;
using UnityEngine;


public class BattleInstantiator : MonoBehaviour
{
    public static BattleInstantiator instance { get; private set; }

    [SerializeField]
    private ArmyModelSO army1Model;

    [SerializeField]
    private ArmyModelSO army2Model;

    [SerializeField]
    private Warrior warriorPrefab;

    [SerializeField]
    private Archer archerPrefab;

    [SerializeField]
    private BoxCollider leftArmySpawnBounds;

    [SerializeField]
    private BoxCollider rightArmySpawnBounds;

    public readonly Army army1 = new Army();
    public readonly Army army2 = new Army();

    public Color army1Color;
    public Color army2Color;

    public GameOverMenu gameOverMenu;

    void InstanceArmy(IArmyModel model, Army army, Bounds instanceBounds)
    {
        for ( int i = 0; i < model.warriors; i++ )
        {
            GameObject go = Instantiate(warriorPrefab.gameObject);
            go.transform.position = Utils.GetRandomPosInBounds(instanceBounds);

            go.GetComponentInChildren<Warrior>().army = army;
            go.GetComponentInChildren<Warrior>().armyModel = model;
            go.GetComponentInChildren<Renderer>().material.color = army.color;

            army.warriors.Add(go.GetComponent<Warrior>());
        }

        for ( int i = 0; i < model.archers; i++ )
        {
            GameObject go = Object.Instantiate(archerPrefab.gameObject);
            go.transform.position = Utils.GetRandomPosInBounds(instanceBounds);

            go.GetComponentInChildren<Archer>().army = army;
            go.GetComponentInChildren<Archer>().armyModel = model;
            go.GetComponentInChildren<Renderer>().material.color = army.color;

            army.archers.Add(go.GetComponent<Archer>());
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
    }

    void Update()
    {
        if ( army1.GetUnits().Count == 0 || army2.GetUnits().Count == 0 )
        {
            gameOverMenu.gameObject.SetActive(true);
            gameOverMenu.Populate();
        }

        Vector3 mainCenter = Utils.GetCenter(army1.GetUnits()) + Utils.GetCenter(army2.GetUnits());
        mainCenter *= 0.5f;

        forwardTarget = (mainCenter - Camera.main.transform.position).normalized;

        Camera.main.transform.forward += (forwardTarget - Camera.main.transform.forward) * 0.1f;
    }

    private Vector3 forwardTarget;
}