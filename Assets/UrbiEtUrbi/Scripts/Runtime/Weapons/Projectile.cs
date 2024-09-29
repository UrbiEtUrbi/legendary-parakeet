using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    protected float Speed;

    protected Vector3 Direction;

    [SerializeField]
    ParticleSystem OnHit;


    public virtual void SetDirection(Vector3 d)
    {

        Direction = d;
    }


    public void BeforeDestroy()
    {
        if (OnHit == null)
        {
            return;
        }
        var t = Instantiate(OnHit);

        if (transform.childCount > 0)
        {
            var child = transform.GetChild(0);

            child.SetParent(null);
            child.transform.localScale = new Vector3(1, 1, 1);
            child.GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
            var dd = child.gameObject.AddComponent<DestroyDelayed>();
            dd.Init(10f);
        }
        t.transform.position = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        BeforeDestroy();
        Destroy(gameObject);

    }
}
