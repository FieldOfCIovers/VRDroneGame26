using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Rendering;

public class sTutorialManager : MonoBehaviour
{
    //Object Pool Variables
    public static sTutorialManager SharedInstance;
    public List<GameObject> pooledObjects;
    //public GameObject objectToPool;
    public int amountToPool;

    //Tutorial Check Variables
    private int ringScore = 0;
    GameObject starterRoom;

    private void Awake()
    {
        SharedInstance = this;
    }

    private void objPoolStart()
    {
        pooledObjects = new List<GameObject>();
        GameObject tmp;
        foreach(GameObject prefab in pooledObjects)
        {
            for (int i = 0; i < amountToPool; i++)
            {
                tmp = Instantiate(prefab);
                tmp.SetActive(false);
                pooledObjects.Add(tmp);
            }
        }
    }

    private GameObject GetObjectsPooled()
    {
        for(int i = 0; i < amountToPool; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }

    private void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        objPoolStart();

        starterRoom = GetObjectsPooled();
        if (starterRoom != null)
        {
            starterRoom.SetActive(true);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (ringScore <= 3)
        {
            ReturnObject(starterRoom);
        }
    }

    void spawnRooms()
    {

    }

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Hit");
    }

}
