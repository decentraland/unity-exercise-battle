using System;
using System.Collections.Generic;
using Exercise.Models.Scripts;
using UnityEngine;

namespace Exercise.Battle.Scripts
{
    public abstract class UnitBase : MonoBehaviour
    {
        private const float MaxRadius = 80.0f;
        public float health { get; protected set; }
        public float defense { get; protected set; }
        public float attack { get; protected set; }
        public float maxAttackCooldown { get; protected set; }
        public float postAttackDelay { get; protected set; }
        public float speed { get; protected set; } = 0.1f;

        public Army army;

        [NonSerialized] public IArmyModel armyModel;

        protected float attackCooldown;
        protected Animator _animator;
        protected Transform _transform;
        private Vector3 lastPosition;

        private static readonly int AnimatorMovementSpeedIndex = Animator.StringToHash("MovementSpeed");
        private static readonly int AnimatorDeathIndex = Animator.StringToHash("Death");
        private static readonly int AnimatorHitIndexHit = Animator.StringToHash("Hit");

        public abstract void Attack(GameObject enemy);

        protected abstract void UpdateDefensive(List<GameObject> enemies, Vector3 toMove);
        protected abstract void UpdateBasic(List<GameObject> enemies, Vector3 toMove);

        public virtual void Move(Vector3 delta)
        {
            if (attackCooldown > maxAttackCooldown - postAttackDelay)
                return;

            _transform.position += delta * speed;
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
                sourceAttack = arrow.Attack;
            }

            health -= Mathf.Max(sourceAttack - defense, 0);

            if (health < 0)
            {
                _transform.forward = sourceGo.transform.position - _transform.position;
                Remove();
                _animator?.SetTrigger(AnimatorDeathIndex);
            }
            else
            {
                _animator?.SetTrigger(AnimatorHitIndexHit);
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

            attackCooldown -= Time.deltaTime;

            //Moving the unit here then use the new position to compute to move again is error-prone
            //It might have edge cases where we move faster than the speed a single frame
            //In addition to the double looping of all the units in an update killing the performance
            var toMove = IsTooFarFromAllies(out var distanceToCenter)
                ? MoveToNearest(distanceToCenter)
                : EvadeAllies();

            var enemies = army.EnemyArmy.Units;

            switch (armyModel.strategy)
            {
                case ArmyStrategy.Defensive:
                    UpdateDefensive(enemies, toMove);
                    break;
                case ArmyStrategy.Basic:
                    UpdateBasic(enemies, toMove);
                    break;
            }

            _animator?.SetFloat(AnimatorMovementSpeedIndex, (_transform.position - lastPosition).magnitude / speed);
            lastPosition = _transform.position;
        }

        private Vector3 EvadeAllies()
        {
            var toMove = Vector3.zero;
            foreach (var unit in army.Units)
            {
                var unitTransform = unit.transform;
                var dist = Vector3.Distance(_transform.position, unitTransform.position);
                if (!(dist < 2f)) continue;
                var toNearest = (unitTransform.position - _transform.position).normalized;
                toMove -= toNearest * (2.0f - dist);
            }

            return toMove;
        }

        private bool IsTooFarFromAllies(out float distanceToCenter)
        {
            distanceToCenter = Vector3.Distance(_transform.position, army.Center);
            return distanceToCenter > MaxRadius;
        }

        private Vector3 MoveToNearest(float distanceToCenter)
        {
            var toNearest = (army.Center - _transform.position).normalized;
            return -toNearest * (MaxRadius - distanceToCenter);
        }
    }
}