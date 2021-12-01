using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPool
{
    public GameObject Prefab => prefab;
    public int Size => size;
    public int RuntimeSize => queue.Count;

    [SerializeField] private GameObject prefab;
    [SerializeField] private int size = 1;

    private Queue<GameObject> queue;

    private Transform parent;

    public void Initialize(Transform parent)
    {
        queue = new Queue<GameObject>();
        this.parent = parent;

        for (var i = 0; i < size; i++)
        {
            queue.Enqueue(InstantiateObject());
        }
    }

    private GameObject InstantiateObject()
    {
        var copy = GameObject.Instantiate(prefab, parent);

        copy.SetActive(false);

        return copy;
    }

    private GameObject GetAvailableObject()
    {
        GameObject availableObject;

        if (queue.Count > 0 && !queue.Peek().activeSelf)
        {
            availableObject = queue.Dequeue();
        }
        else
        {
            availableObject = InstantiateObject();
        }

        queue.Enqueue(availableObject);

        return availableObject;
    }

    public GameObject PreparedObject()
    {
        GameObject preparedObject = GetAvailableObject();

        preparedObject.SetActive(true);

        return preparedObject;
    }

    public GameObject PreparedObject(Vector3 position)
    {
        GameObject preparedObject = GetAvailableObject();

        preparedObject.SetActive(true);
        preparedObject.transform.position = position;

        return preparedObject;
    }

    public GameObject[] PreparedObject(Vector3[] positions)
    {
        GameObject[] preparedObjects = new GameObject[positions.Length];

        for (int i = 0; i < positions.Length; i++)
        {
            preparedObjects[i] = GetAvailableObject();
            preparedObjects[i].SetActive(true);
            preparedObjects[i].transform.position = positions[i];
        }

        return preparedObjects;
    }

    public GameObject PreparedObject(Vector3 position, Quaternion rotation)
    {
        GameObject preparedObject = GetAvailableObject();

        preparedObject.SetActive(true);
        preparedObject.transform.position = position;
        preparedObject.transform.rotation = rotation;

        return preparedObject;
    }

    public GameObject PreparedObject(Vector3 position, Quaternion rotation, Vector3 localScale)
    {
        GameObject preparedObject = GetAvailableObject();

        preparedObject.SetActive(true);
        preparedObject.transform.position = position;
        preparedObject.transform.rotation = rotation;
        preparedObject.transform.localScale = localScale;

        return preparedObject;
    }
}