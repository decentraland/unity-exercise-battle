using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Archer : UnitBase
{
    public float attackRange = 20f;

    public ArcherArrow arrowPrefab;

    void Awake()
    {
        health = 5;
        defense = 0;
        attack = 10;
        maxAttackCooldown = 5f;
        postAttackDelay = 1f;
    }

    public override void Attack(GameObject enemy)
    {
        if ( attackCooldown > 0 )
            return;

        if ( Vector3.Distance(transform.position, enemy.transform.position) > attackRange )
            return;

        attackCooldown = maxAttackCooldown;
        GameObject arrow = Object.Instantiate(arrowPrefab.gameObject);
        arrow.GetComponent<ArcherArrow>().target = enemy.transform.position;
        arrow.GetComponent<ArcherArrow>().attack = attack;
        arrow.GetComponent<ArcherArrow>().army = army;
        arrow.transform.position = transform.position;

        var animator = GetComponentInChildren<Animator>();
        animator?.SetTrigger("Attack");

        if ( army == BattleInstantiator.instance.army1 )
            arrow.GetComponent<Renderer>().material.color = BattleInstantiator.instance.army1Color;
        else
            arrow.GetComponent<Renderer>().material.color = BattleInstantiator.instance.army2Color;
    }

    public void OnDeathAnimFinished()
    {
        Destroy(gameObject);
    }

    protected override void UpdateDefensive(List<GameObject> allies, List<GameObject> enemies)
    {
        Vector3 enemyCenter = Utils.GetCenter(enemies);
        float distToEnemyX = Mathf.Abs( enemyCenter.x - transform.position.x );

        if ( distToEnemyX > attackRange )
        {
            if ( enemyCenter.x < transform.position.x )
                Move( Vector3.left );

            if ( enemyCenter.x > transform.position.x )
                Move( Vector3.right );
        }

        float distToNearest = Utils.GetNearestObject(gameObject, enemies, out GameObject nearestEnemy );

        if ( nearestEnemy == null )
            return;

        if ( distToNearest < attackRange )
        {
            Vector3 toNearest = (nearestEnemy.transform.position - transform.position).normalized;
            toNearest.Scale( new Vector3(1, 0, 1));

            Vector3 flank = Quaternion.Euler(0, 90, 0) * toNearest;
            Move( -(toNearest + flank).normalized );
        }
        else
        {
            Vector3 toNearest = (nearestEnemy.transform.position - transform.position).normalized;
            toNearest.Scale( new Vector3(1, 0, 1));
            Move( toNearest.normalized );
        }

        Attack(nearestEnemy);
    }

    protected override void UpdateBasic(List<GameObject> allies, List<GameObject> enemies)
    {
        Utils.GetNearestObject(gameObject, enemies, out GameObject nearestEnemy );

        if ( nearestEnemy == null )
            return;

        Vector3 toNearest = (nearestEnemy.transform.position - transform.position).normalized;
        toNearest.Scale( new Vector3(1, 0, 1));
        Move( toNearest.normalized );

        Attack(nearestEnemy);
    }
}