using UnityEngine;

namespace Exercise.Battle.Scripts
{
    public sealed class Movement : MonoBehaviour
    {
        public float Speed;
        private Transform _transform;

        private void Awake() => _transform = transform;

        public void Move(Vector3 delta)
        {
            _transform.position += delta * Speed;
        }
    }
}