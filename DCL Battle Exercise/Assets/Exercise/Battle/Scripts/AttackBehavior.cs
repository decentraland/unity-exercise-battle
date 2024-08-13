using UnityEngine;

namespace Exercise.Battle.Scripts
{
    [RequireComponent(typeof(Unit))]
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(Defense))]
    [RequireComponent(typeof(Movement))]
    [RequireComponent(typeof(Attack))]
    [RequireComponent(typeof(Animator))]
    public abstract class AttackBehavior : MonoBehaviour
    {
        protected static readonly int AnimatorAttackIndex = Animator.StringToHash("Attack");

        protected Army _army;
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

        public void Setup() => _army = GetComponent<Unit>().Army;

        public virtual void UpdateStrategy(Vector3 toMove)
        {
            _attackCooldown -= Time.deltaTime;
            //TODO: Moving here is error-prone since it might move faster than the speed of the current frame
            _transform.position += toMove;
        }

        protected bool CanMove() => _attackCooldown < _attack.MaxCooldown - _attack.PostDelay;
    }
}