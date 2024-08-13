using System;
using Exercise.Models.Scripts;
using UnityEngine;

namespace Exercise.Battle.Scripts
{
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(Defense))]
    [RequireComponent(typeof(Attack))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Movement))]
    public abstract class UnitBase : MonoBehaviour
    {
        private static readonly int AnimatorMovementSpeedIndex = Animator.StringToHash("MovementSpeed");
        private static readonly int AnimatorDeathIndex = Animator.StringToHash("Death");
        private static readonly int AnimatorHitIndexHit = Animator.StringToHash("Hit");

        private const float MaxRadius = 80.0f;

        protected Army _army;
        protected AttackBehavior _attackBehavior;

        private Transform _transform;
        private Animator _animator;
        private Health _health;
        private Defense _defense;
        private Movement _movement;
        private Vector3 _lastPositionForAnimation;

        protected virtual void Awake()
        {
            _health = GetComponent<Health>();
            _defense = GetComponent<Defense>();
            _movement = GetComponent<Movement>();
            _animator = GetComponentInChildren<Animator>();
            _transform = transform;
        }

        public void Hit(GameObject attackerGo)
        {
            var attackerDamage = attackerGo.GetComponent<Damage>();
            var attackerDamageValue = attackerDamage.Value;

            _health.Value -= Mathf.Max(attackerDamageValue - _defense.Value, 0);

            if (_health.Value < 0)
            {
                _transform.forward = attackerGo.transform.position - _transform.position;
                Remove();
                _animator?.SetTrigger(AnimatorDeathIndex);
            }
            else
            {
                _animator?.SetTrigger(AnimatorHitIndexHit);
            }
        }

        public void OnDeathAnimFinished() => Kill();

        private void Kill()
        {
            Remove();
            Destroy(gameObject);
        }

        private void Remove()
        {
            switch (this)
            {
                case Warrior:
                    _army.Remove(this as Warrior);
                    break;
                case Archer:
                    _army.Remove(this as Archer);
                    break;
            }
        }

        private void Update()
        {
            if (_health.Value < 0)
                return;

            var toMove = IsTooFarFromAllies(out var distanceToCenter)
                ? MoveToNearest(distanceToCenter)
                : EvadeAllies();

            _attackBehavior.UpdateStrategy(toMove);

            _animator?.SetFloat(AnimatorMovementSpeedIndex,
                (_transform.position - _lastPositionForAnimation).magnitude / _movement.Speed);
            _lastPositionForAnimation = _transform.position;
        }

        private Vector3 EvadeAllies()
        {
            var toMove = Vector3.zero;
            foreach (var unitTransform in _army.Transforms)
            {
                var dist = Vector3.Distance(_transform.position, unitTransform.position);
                if (!(dist < 2f)) continue;
                var toNearest = (unitTransform.position - _transform.position).normalized;
                toMove -= toNearest * (2.0f - dist);
            }
            return toMove;
        }

        private bool IsTooFarFromAllies(out float distanceToCenter)
        {
            distanceToCenter = Vector3.Distance(_transform.position, _army.Center);
            return distanceToCenter > MaxRadius;
        }

        private Vector3 MoveToNearest(float distanceToCenter)
        {
            var toNearest = (_army.Center - _transform.position).normalized;
            return -toNearest * (MaxRadius - distanceToCenter);
        }
    }
}