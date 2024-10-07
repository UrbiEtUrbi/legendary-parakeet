using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    protected float Speed;

    protected Vector3 Direction;

    [SerializeField]
    GameObject OnHit;

    protected Vector3 HitPosition;


    public virtual void SetDirection(Vector3 d)
    {

        Direction = d;
    }


    public virtual void BeforeDestroy()
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
        HitPosition = new Vector3(collision.contacts[0].point.x, collision.contacts[0].point.y, 0);
        BeforeDestroy();
        Destroy(gameObject);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Bounds triggerBounds = GetComponent<Collider2D>().bounds;
        Bounds colliderBounds = collision.bounds;

        // Calculate the center of the intersection of the two bounds
        float intersectX = Mathf.Max(triggerBounds.min.x, colliderBounds.min.x);
        float intersectY = Mathf.Max(triggerBounds.min.y, colliderBounds.min.y);

        // This should give you an intersection point between the two colliders
        Vector2 intersectionPoint = new Vector2(intersectX, intersectY);
        HitPosition = intersectionPoint;
        //HitPosition = new Vector3(collision.contacts[0].point.x, collision.contacts[0].point.y, 0);
        BeforeDestroy();
        Destroy(gameObject);

    }
}
