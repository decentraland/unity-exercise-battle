using System;
using System.Collections.Generic;
using System.Linq;
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

    [NonSerialized]
    public IArmyModel armyModel;

    protected float attackCooldown;
    private Vector3 lastPosition;

    public abstract void Attack(GameObject enemy);


    protected abstract void UpdateDefensive(List<GameObject> allies, List<GameObject> enemies);
    protected abstract void UpdateBasic(List<GameObject> allies, List<GameObject> enemies);

    public virtual void Move( Vector3 delta )
    {
        if (attackCooldown > maxAttackCooldown - postAttackDelay)
            return;

        transform.position += delta * speed;
    }

    public virtual void Hit( GameObject sourceGo )
    {
        UnitBase source = sourceGo.GetComponent<UnitBase>();
        float sourceAttack = 0;

        if ( source != null )
        {
            sourceAttack = source.attack;
        }
        else
        {
            ArcherArrow arrow = sourceGo.GetComponent<ArcherArrow>();
            sourceAttack = arrow.attack;
        }

        health -= Mathf.Max(sourceAttack - defense, 0);

        if ( health < 0 )
        {
            transform.forward = sourceGo.transform.position - transform.position;

            if ( this is Warrior )
                army.warriors.Remove(this as Warrior);
            else if ( this is Archer )
                army.archers.Remove(this as Archer);

            var animator = GetComponentInChildren<Animator>();
            animator?.SetTrigger("Death");
        }
        else
        {
            var animator = GetComponentInChildren<Animator>();
            animator?.SetTrigger("Hit");
        }
    }

    private void Update()
    {
        if ( health < 0 )
            return;

        List<GameObject> allies = army.GetUnits();
        List<GameObject> enemies = army.enemyArmy.GetUnits();

        UpdateBasicRules(allies, enemies);

        switch ( armyModel.strategy )
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

    void UpdateBasicRules(List<GameObject> allies, List<GameObject> enemies)
    {
        attackCooldown -= Time.deltaTime;
        EvadeAllies(allies);
    }

    void EvadeAllies(List<GameObject> allies)
    {
        var allUnits = army.GetUnits().Union(army.enemyArmy.GetUnits()).ToList();

        Vector3 center = Utils.GetCenter(allUnits);

        float centerDist = Vector3.Distance(gameObject.transform.position, center);

        if ( centerDist > 80.0f )
        {
            Vector3 toNearest = (center - transform.position).normalized;
            transform.position -= toNearest * (80.0f - centerDist);
            return;
        }

        foreach ( var obj in allUnits )
        {
            float dist = Vector3.Distance(gameObject.transform.position, obj.transform.position);

            if ( dist < 2f )
            {
                Vector3 toNearest = (obj.transform.position - transform.position).normalized;
                transform.position -= toNearest * (2.0f - dist);
            }
        }
    }
}