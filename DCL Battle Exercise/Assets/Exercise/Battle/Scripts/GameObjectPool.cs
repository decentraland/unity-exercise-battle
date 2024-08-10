using UnityEngine;
using UnityEngine.Pool;

namespace Exercise.Battle.Scripts
{
    public sealed class GameObjectPool
    {
        private readonly GameObject prefab;
        private readonly ObjectPool<GameObject> pool;

        public GameObjectPool(GameObject prefab, int defaultSize, int maxSize)
        {
            this.prefab = prefab;
            pool = new ObjectPool<GameObject>(
                CreatePooledObject,
                OnGetFromPool,
                OnReturnToPool,
                OnDestroyPooledObject,
                true,
                defaultSize,
                maxSize
            );
        }

        public GameObject GetObject(Vector3 position)
        {
            var gameObject = pool.Get();
            gameObject.transform.position = position;
            return gameObject;
        }

        public void ReleaseObject(GameObject gameObject) => pool.Release(gameObject);

        private GameObject CreatePooledObject() => GameObject.Instantiate(prefab);

        private static void OnGetFromPool(GameObject pooledObject) => pooledObject.SetActive(true);

        private static void OnReturnToPool(GameObject pooledObject) => pooledObject.SetActive(false);

        private static void OnDestroyPooledObject(GameObject pooledObject) => GameObject.Destroy(pooledObject);
    }
}