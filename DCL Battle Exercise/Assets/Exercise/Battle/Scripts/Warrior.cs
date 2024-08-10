using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class Warrior : UnitBase
{
    [NonSerialized] public float attackRange = 2.5f;
    private static readonly int AnimatorAttackIndex = Animator.StringToHash("Attack");

    private void Awake()
    {
        health = 50;
        defense = 5;
        attack = 20;
        maxAttackCooldown = 1f;
        postAttackDelay = 0;
        _animator = GetComponentInChildren<Animator>();
        _transform = transform;
    }

    public override void Attack(GameObject target)
    {
        if (attackCooldown > 0)
            return;

        if (Vector3.Distance(transform.position, target.transform.position) > attackRange)
            return;

        var targetUnit = target.GetComponentInChildren<UnitBase>();

        if (targetUnit == null)
            return;

        attackCooldown = maxAttackCooldown;

        _animator?.SetTrigger(AnimatorAttackIndex);

        targetUnit.Hit(gameObject);
    }

    public void OnDeathAnimFinished() => Kill();

    protected override void UpdateDefensive(List<GameObject> enemies, Vector3 toMove)
    {
        var enemyCenter = army.enemyArmy.center;
        
        _transform.position += toMove;

        if (Mathf.Abs(enemyCenter.x - _transform.position.x) > 20)
        {
            if (enemyCenter.x < _transform.position.x)
                Move(Vector3.left);

            if (enemyCenter.x > _transform.position.x)
                Move(Vector3.right);
        }

        Utils.GetNearestObject(gameObject, enemies, out var nearestObject);

        if (nearestObject == null)
            return;

        if (attackCooldown <= 0)
            Move((nearestObject.transform.position - _transform.position).normalized);
        else
        {
            Move((nearestObject.transform.position - _transform.position).normalized * -1);
        }

        Attack(nearestObject);
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