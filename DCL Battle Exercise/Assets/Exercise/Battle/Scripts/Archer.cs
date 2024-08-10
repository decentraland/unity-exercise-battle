using System.Collections.Generic;
using UnityEngine;

public sealed class Archer : UnitBase
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
        if (attackCooldown > 0)
            return;

        if (Vector3.Distance(transform.position, enemy.transform.position) > attackRange)
            return;

        attackCooldown = maxAttackCooldown;
        var arrowGameObject = Object.Instantiate(arrowPrefab.gameObject);
        var archerArrow = arrowGameObject.GetComponent<ArcherArrow>();
        archerArrow.target = enemy.transform.position;
        archerArrow.attack = attack;
        archerArrow.army = army;
        arrowGameObject.transform.position = transform.position;

        var animator = GetComponentInChildren<Animator>();
        animator?.SetTrigger("Attack");

        arrowGameObject.GetComponent<Renderer>().material.color = army == BattleInstantiator.instance.army1
            ? BattleInstantiator.instance.army1Color
            : BattleInstantiator.instance.army2Color;
    }

    public void OnDeathAnimFinished() => Kill();
    
    protected override void UpdateDefensive(List<GameObject> allies, List<GameObject> enemies)
    {
        var enemyCenter = Utils.GetCenter(enemies);
        var distToEnemyX = Mathf.Abs(enemyCenter.x - transform.position.x);

        if (distToEnemyX > attackRange)
        {
            if (enemyCenter.x < transform.position.x)
                Move(Vector3.left);

            if (enemyCenter.x > transform.position.x)
                Move(Vector3.right);
        }

        var distToNearest = Utils.GetNearestObject(gameObject, enemies, out GameObject nearestEnemy);

        if (nearestEnemy == null)
            return;

        if (distToNearest < attackRange)
        {
            var toNearest = (nearestEnemy.transform.position - transform.position).normalized;
            toNearest.Scale(new Vector3(1, 0, 1));

            var flank = Quaternion.Euler(0, 90, 0) * toNearest;
            Move(-(toNearest + flank).normalized);
        }
        else
        {
            var toNearest = (nearestEnemy.transform.position - transform.position).normalized;
            toNearest.Scale(new Vector3(1, 0, 1));
            Move(toNearest.normalized);
        }

        Attack(nearestEnemy);
    }

    protected override void UpdateBasic(List<GameObject> allies, List<GameObject> enemies)
    {
        Utils.GetNearestObject(gameObject, enemies, out GameObject nearestEnemy);

        if (nearestEnemy == null)
            return;

        var toNearest = (nearestEnemy.transform.position - transform.position).normalized;
        toNearest.Scale(new Vector3(1, 0, 1));
        Move(toNearest.normalized);

        Attack(nearestEnemy);
    }
}