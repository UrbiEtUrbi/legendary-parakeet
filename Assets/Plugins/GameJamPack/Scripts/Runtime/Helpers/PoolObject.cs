using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject : MonoBehaviour
{

    [HideInInspector]
    public string key;


    public void Despawn()
    {
        PoolManager.Despawn(this);
    }

    /// <summary>
    /// This is called when the pooled object is at the start of new lifecycle
    /// Useful for initializing
    /// </summary>
    public virtual void Reuse()
    {
        this.gameObject.SetActive(true);
    }



  

    /// <summary>
    /// This is called when the pooled object is at the end of lifecycle
    /// </summary>
    public virtual void Destroy()
    {
        this.gameObject.SetActive(false);
    }
}
