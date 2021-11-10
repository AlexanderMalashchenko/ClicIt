using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public class ObjectPool : MonoBehaviour
    {
        private Queue<GameObject> objects = new Queue<GameObject>();

        public void Prewarm(int count, GameObject prefab)
        {
            if (objects.Count >= count)
            {
                Debug.LogWarning("Pool is already big enough!");
                return;
            }

            int numberToPrewarm = count - objects.Count;
            AddObjects(numberToPrewarm, prefab);

        }

        public virtual GameObject Get(GameObject prefab)
        {
            if (objects.Count == 0)
                AddObjects(1, prefab);
            GameObject objectFromPool = objects.Dequeue();
            return objectFromPool;
        }




        public void AddObjects(int count, GameObject prefab)
        {
            for (int i = 0; i < count; i++)
            {
                var newObject = GameObject.Instantiate(prefab);
                newObject.gameObject.SetActive(false);
                newObject.transform.SetParent(transform);
                objects.Enqueue(newObject);
            }
        }

        public virtual void ReturnToPool(GameObject objectToReturn)
        {
            objectToReturn.gameObject.SetActive(false);
            objects.Enqueue(objectToReturn);
        }
    }
}