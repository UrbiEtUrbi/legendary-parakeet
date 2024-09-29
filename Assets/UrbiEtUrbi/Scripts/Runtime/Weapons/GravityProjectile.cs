using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityProjectile : Projectile
{

    Rigidbody2D rb;

    [SerializeField]
    Transform View;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public override void SetDirection(Vector3 d)
    {
        rb.velocity = d * Speed;
    }

    private void FixedUpdate()
    {
        if (View)
        {
            View.right = rb.velocity.normalized;
        }
    }

}
