using System.Collections.Generic;
using UnityEngine;

public sealed class Archer : UnitBase
{
    public float attackRange = 20f;
    private static readonly int AnimatorAttackIndex = Animator.StringToHash("Attack");

    private void Awake()
    {
        health = 5;
        defense = 0;
        attack = 10;
        maxAttackCooldown = 5f;
        postAttackDelay = 1f;
        _animator = GetComponentInChildren<Animator>();
        _transform = transform;
    }

    public override void Attack(GameObject enemy)
    {
        if (attackCooldown > 0)
            return;

        if (Vector3.Distance(transform.position, enemy.transform.position) > attackRange)
            return;

        attackCooldown = maxAttackCooldown;
        var arrowGo = army.Pool.GetObject(transform.position);
        var archerArrow = arrowGo.GetComponent<ArcherArrow>();
        archerArrow.target = enemy.transform.position;
        archerArrow.attack = attack;
        archerArrow.army = army;

        _animator?.SetTrigger(AnimatorAttackIndex);

        arrowGo.GetComponent<Renderer>().material.color = army == BattleInstantiator.instance.Army1
            ? BattleInstantiator.instance.army1Color
            : BattleInstantiator.instance.army2Color;
    }

    public void OnDeathAnimFinished() => Kill();

    protected override void UpdateDefensive(List<GameObject> enemies, Vector3 toMove)
    {
        var enemyCenter = army.enemyArmy.center;
        _transform.position += toMove;
        var distToEnemyX = Mathf.Abs(enemyCenter.x - _transform.position.x);

        if (distToEnemyX > attackRange)
        {
            if (enemyCenter.x < _transform.position.x)
                Move(Vector3.left);
            if (enemyCenter.x > _transform.position.x)
                Move(Vector3.right);
        }

        var distToNearest = Utils.GetNearestObject(gameObject, enemies, out var nearestEnemy);

        if (nearestEnemy == null)
            return;

        if (distToNearest < attackRange)
        {
            var toNearest = (nearestEnemy.transform.position - _transform.position).normalized;
            toNearest.Scale(new Vector3(1, 0, 1));

            var flank = Quaternion.Euler(0, 90, 0) * toNearest;
            Move(-(toNearest + flank).normalized);
        }
        else
        {
            var toNearest = (nearestEnemy.transform.position - _transform.position).normalized;
            toNearest.Scale(new Vector3(1, 0, 1));
            Move(toNearest.normalized);
        }

        Attack(nearestEnemy);
    }

    protected override void UpdateBasic(List<GameObject> enemies, Vector3 toMove)
    {
        _transform.position += toMove;
        Utils.GetNearestObject(gameObject, enemies, out var nearestEnemy);

        if (nearestEnemy == null)
            return;

        var toNearest = (nearestEnemy.transform.position - _transform.position).normalized;
        toNearest.Scale(new Vector3(1, 0, 1));
        Move(toNearest.normalized);
        Attack(nearestEnemy);
    }
}