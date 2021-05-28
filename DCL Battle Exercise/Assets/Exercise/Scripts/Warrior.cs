using System.Collections.Generic;
using UnityEngine;

public interface IUnit
{
    float health { get; set; }
    float defense { get; set; }
    float attack { get; set; }
}

public class Warrior : MonoBehaviour, IUnit
{
    public Army allyArmy;
    public Army enemyArmy;

    public float speed;
    public float attackCooldown = 0.5f;

    private void Update()
    {
        var settings = BattleManager.instance.settings;
        List<GameObject> allies, enemies;

        if ( allyArmy == BattleManager.instance.army1 )
        {
            allies = BattleManager.instance.army1.GetUnits();
            enemies = BattleManager.instance.army2.GetUnits();
            enemyArmy = BattleManager.instance.army2;
        }
        else
        {
            allies = BattleManager.instance.army2.GetUnits();
            enemies = BattleManager.instance.army1.GetUnits();
            enemyArmy = BattleManager.instance.army1;
        }

        switch ( settings.armyStrategy )
        {
            case BattleSettings.ArmyStrategy.StrategyDefensive:
                UpdateDefensive(allies, enemies);
                break;
            case BattleSettings.ArmyStrategy.StrategyFlank:
                UpdateFlank(allies, enemies);
                break;
        }
    }

    void UpdateDefensive(List<GameObject> allies, List<GameObject> enemies)
    {
        foreach ( var obj in allies )
        {
            float dist = Vector3.Distance(gameObject.transform.position, obj.transform.position);

            if ( dist < 2.0f )
            {
                Vector3 toNearest = (obj.transform.position - transform.position).normalized;
                transform.position -= toNearest * (2.0f - dist);
            }
        }

        Vector3 enemyCenter = Utils.GetCenter(enemies);

        if ( Mathf.Abs( enemyCenter.x - transform.position.x ) > 20 )
        {
            if ( enemyCenter.x < transform.position.x )
                transform.position -= Vector3.right * speed;

            if ( enemyCenter.x > transform.position.x )
                transform.position += Vector3.right * speed;
        }

        float distToNearest = Utils.GetNearestObject(gameObject, enemies, out GameObject nearestObject );

        if ( nearestObject == null )
            return;

        if ( attackCooldown <= 0 )
            transform.position += (nearestObject.transform.position - transform.position).normalized * speed;
        else if ( attackCooldown > 0 )
            transform.position -= (nearestObject.transform.position - transform.position).normalized * speed;

        attackCooldown -= Time.deltaTime;
        // Unit Attack
        if ( distToNearest < 2.0 && attackCooldown <= 0 )
        {
            IUnit targetUnit = nearestObject.GetComponent<IUnit>();

            if ( targetUnit == null )
                return;

            targetUnit.health -= Mathf.Max(attack - defense, 0);
            attackCooldown = 0.5f;

            if ( targetUnit.health < 0 )
            {
                enemyArmy.warriors.Remove(nearestObject.GetComponent<Warrior>());
                enemyArmy.archers.Remove(nearestObject.GetComponent<Archer>());
                Destroy( nearestObject );
            }
        }
    }

    void UpdateFlank(List<GameObject> allies, List<GameObject> enemies)
    {
    }

    public float health { get; set; } = 50;
    public float defense { get; set; } = 5;
    public float attack { get; set; } = 20;
}