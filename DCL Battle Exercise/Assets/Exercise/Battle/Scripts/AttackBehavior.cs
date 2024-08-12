using System.Collections.Generic;
using UnityEngine;

namespace Exercise.Battle.Scripts
{
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(Defense))]
    [RequireComponent(typeof(Movement))]
    [RequireComponent(typeof(Attack))]
    [RequireComponent(typeof(Animator))]
    public abstract class AttackBehavior : MonoBehaviour
    {
        protected static readonly int AnimatorAttackIndex = Animator.StringToHash("Attack");

        public Army Army;

        protected Transform _transform;
        protected Animator _animator;
        protected Health _health;
        protected Attack _attack;
        protected Movement _movement;
        protected float _attackCooldown;

        protected virtual void Awake()
        {
            _health = GetComponent<Health>();
            _attack = GetComponent<Attack>();
            _movement = GetComponent<Movement>();
            _animator = GetComponentInChildren<Animator>();
            _transform = transform;
        }

        public virtual void UpdateStrategy(List<GameObject> enemies, Vector3 toMove) =>
            _attackCooldown -= Time.deltaTime;

        protected bool CanMove() => _attackCooldown < _attack.MaxCooldown - _attack.PostDelay;
    }
}