using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitBase : MonoBehaviour
{
    public float health { get; protected set; }
    public float defense { get; protected set; }
    public float attack { get; protected set; }
    public float maxAttackCooldown { get; protected set; }
    public float postAttackDelay { get; protected set; }
    public float speed { get; protected set; } = 0.1f;

    public Army army;

    [NonSerialized] public IArmyModel armyModel;

    protected float attackCooldown;
    private Vector3 lastPosition;

    public abstract void Attack(GameObject enemy);

    protected abstract void UpdateDefensive(List<GameObject> allies, List<GameObject> enemies);
    protected abstract void UpdateBasic(List<GameObject> allies, List<GameObject> enemies);

    public virtual void Move(Vector3 delta)
    {
        if (attackCooldown > maxAttackCooldown - postAttackDelay)
            return;

        transform.position += delta * speed;
    }

    public virtual void Hit(GameObject sourceGo)
    {
        var source = sourceGo.GetComponent<UnitBase>();
        float sourceAttack = 0;

        if (source != null)
        {
            sourceAttack = source.attack;
        }
        else
        {
            var arrow = sourceGo.GetComponent<ArcherArrow>();
            sourceAttack = arrow.attack;
        }

        health -= Mathf.Max(sourceAttack - defense, 0);

        if (health < 0)
        {
            transform.forward = sourceGo.transform.position - transform.position;
            Remove();
            var animator = GetComponentInChildren<Animator>();
            animator?.SetTrigger("Death");
        }
        else
        {
            var animator = GetComponentInChildren<Animator>();
            animator?.SetTrigger("Hit");
        }
    }

    protected void Kill()
    {
        Remove();
        Destroy(gameObject);
    }

    private void Remove()
    {
        switch (this)
        {
            case Warrior:
                army.Remove(this as Warrior);
                break;
            case Archer:
                army.Remove(this as Archer);
                break;
        }
    }

    private void Update()
    {
        if (health < 0)
            return;

        UpdateBasicRules();

        var allies = army.Units;
        var enemies = army.enemyArmy.Units;

        switch (armyModel.strategy)
        {
            case ArmyStrategy.Defensive:
                UpdateDefensive(allies, enemies);
                break;
            case ArmyStrategy.Basic:
                UpdateBasic(allies, enemies);
                break;
        }

        var animator = GetComponentInChildren<Animator>();
        animator.SetFloat("MovementSpeed", (transform.position - lastPosition).magnitude / speed);
        lastPosition = transform.position;
    }

    private void UpdateBasicRules()
    {
        attackCooldown -= Time.deltaTime;
        EvadeAllies();
    }

    private void EvadeAllies()
    {
        var centerDist = Vector3.Distance(gameObject.transform.position, army.center);

        if (centerDist > 80.0f)
        {
            var toNearest = (army.center - transform.position).normalized;
            transform.position -= toNearest * (80.0f - centerDist);
            return;
        }

        foreach (var unit in army.Units)
        {
            var dist = Vector3.Distance(gameObject.transform.position, unit.transform.position);
            if (!(dist < 2f)) continue;
            var toNearest = (unit.transform.position - transform.position).normalized;
            transform.position -= toNearest * (2.0f - dist);
        }
    }
}