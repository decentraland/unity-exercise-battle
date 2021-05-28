using System.Collections;
using UnityEditor;
using UnityEngine;


public class BattleManager : MonoBehaviour
{
    public static BattleManager instance { get; private set; }

    public BattleSettings settings;

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

    void InstanceArmy(BattleSettings.ArmySettings settings, Army army, Bounds instanceBounds)
    {
        for ( int i = 0; i < settings.warriorCount; i++ )
        {
            GameObject go = Instantiate(warriorPrefab.gameObject);
            Vector3 pos = Vector3.zero;
            pos.x = Random.Range( instanceBounds.min.x, instanceBounds.max.x );
            pos.z = Random.Range( instanceBounds.min.z, instanceBounds.max.z );
            go.transform.position = pos;

            go.GetComponent<Warrior>().allyArmy = army;
            go.GetComponent<Warrior>().settings = settings;
            go.GetComponent<Renderer>().material.color = army.color;

            army.warriors.Add(go.GetComponent<Warrior>());
        }

        for ( int i = 0; i < settings.archerCount; i++ )
        {
            GameObject go = Object.Instantiate(archerPrefab.gameObject);
            Vector3 pos = Vector3.zero;
            pos.x = Random.Range( instanceBounds.min.x, instanceBounds.max.x );
            pos.z = Random.Range( instanceBounds.min.z, instanceBounds.max.z );
            go.transform.position = pos;

            go.GetComponent<Archer>().allyArmy = army;
            go.GetComponent<Archer>().settings = settings;
            go.GetComponent<Renderer>().material.color = army.color;

            army.archers.Add(go.GetComponent<Archer>());
        }
    }

    void Awake()
    {
        instance = this;

        army1.color = army1Color;
        army2.color = army2Color;

        InstanceArmy(settings.army1Settings, army1, leftArmySpawnBounds.bounds);
        InstanceArmy(settings.army2Settings, army2, rightArmySpawnBounds.bounds);
    }

    void Update()
    {
        if ( army1.GetUnits().Count == 0 )
        {
            Debug.Log("Army 2 wins!!");
        }

        if ( army2.GetUnits().Count == 0 )
        {
            Debug.Log("Army 1 wins!!");
        }
    }
}