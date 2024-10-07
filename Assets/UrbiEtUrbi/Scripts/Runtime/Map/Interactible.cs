using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactible : MonoBehaviour
{


   
    public float InteractTime;
    public bool DestroyOnInteract;

    Material m;

    protected virtual bool CanInteract()
    {

        return true;
    }

    private void Awake()
    {
        var sr = GetComponent<SpriteRenderer>();
        m = new Material(sr.sharedMaterial);
        sr.sharedMaterial = m;
    }



    public virtual void OnInteract()
    {


        if (DestroyOnInteract)
        {
            Destroy(gameObject);
        }

    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && CanInteract())
        {
            m.SetFloat("_Radius", 1f);
            TheGame.Instance.ControllerInteractibles.Add(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        m.SetFloat("_Radius", 0);
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            TheGame.Instance.ControllerInteractibles.Remove(this);
        }
    }

   
}
