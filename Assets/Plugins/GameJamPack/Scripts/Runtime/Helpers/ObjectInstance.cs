using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInstance
{
    GameObject gameObject;
    Transform transform;


    

    PoolObject poolObject;
    public PoolObject PoolObject => poolObject;

    public ObjectInstance(GameObject gameObject, string key)
    {
        this.gameObject = gameObject;
        this.transform = gameObject.transform;
        this.gameObject.SetActive(false);

        poolObject = gameObject.GetComponent<PoolObject>();
        poolObject.key = key;
        
    }

    public void Reuse(Transform parent, Vector3 position,Quaternion rotation)
    {
        transform.SetParent(parent);
        if (parent)
        {
            gameObject.transform.localPosition = position;
            gameObject.transform.localRotation = rotation;
        }
        else
        {
            gameObject.transform.position = position;
            gameObject.transform.rotation = rotation;
        }
        poolObject.Reuse();
    }

    public void Destroy()
    {
        poolObject.Destroy();
    }
    

    public void SetParent(Transform parent)
    {
        gameObject.transform.SetParent(parent);
    }
}
