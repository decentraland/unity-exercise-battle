using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Archer : MonoBehaviour, IUnit
{
    public Army allyArmy;
    public Army enemyArmy;

    public float speed;
    public float attackCooldown = 2.0f;

    public ArcherArrow arrowPrefab;

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
        attackCooldown -= Time.deltaTime;

        foreach ( var obj in allies )
        {
            float dist = Vector3.Distance(gameObject.transform.position, obj.transform.position);

            if ( dist < 2.0f )
            {
                Vector3 toNearest = (obj.transform.position - transform.position).normalized;
                transform.position -= toNearest * (2.0f - dist);
            }
        }

        if ( attackCooldown > 4f )
            return;

        Vector3 enemyCenter = Utils.GetCenter(enemies);
        float distToEnemyX = Mathf.Abs( enemyCenter.x - transform.position.x );

        if ( distToEnemyX > 20 )
        {
            if ( enemyCenter.x < transform.position.x )
                transform.position -= Vector3.right * speed;

            if ( enemyCenter.x > transform.position.x )
                transform.position += Vector3.right * speed;
        }

        float distToNearest = Utils.GetNearestObject(gameObject, enemies, out GameObject nearestEnemy );

        if ( nearestEnemy == null )
            return;

        if ( distToNearest < 20.0 )
        {
            Vector3 toNearest = (nearestEnemy.transform.position - transform.position).normalized;
            toNearest.Scale( new Vector3(1, 0, 1));

            Vector3 flank = Quaternion.Euler(0, 90, 0) * toNearest;
            transform.position -= (toNearest + flank).normalized * speed;
        }
        else
        {
            Vector3 toNearest = (nearestEnemy.transform.position - transform.position).normalized;
            toNearest.Scale( new Vector3(1, 0, 1));
            transform.position += toNearest.normalized * speed;
        }


        if ( distToNearest < 20.0 && attackCooldown < 0)
        {
            attackCooldown = 5.0f;
            GameObject arrow = Object.Instantiate(arrowPrefab.gameObject);
            arrow.GetComponent<ArcherArrow>().target = nearestEnemy.transform.position;
            arrow.GetComponent<ArcherArrow>().attack = attack;
            arrow.GetComponent<ArcherArrow>().allyArmy = allyArmy;
            arrow.GetComponent<ArcherArrow>().enemyArmy = enemyArmy;
            arrow.transform.position = transform.position;

            if ( allyArmy == BattleManager.instance.army1 )
                arrow.GetComponent<Renderer>().material.color = BattleManager.instance.army1Color;
            else
                arrow.GetComponent<Renderer>().material.color = BattleManager.instance.army2Color;
        }
    }

    void UpdateFlank(List<GameObject> allies, List<GameObject> enemies)
    {
    }

    public float health { get; set; } = 5;
    public float defense { get; set; } = 0;
    public float attack { get; set; } = 5;
}