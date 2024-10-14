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

    public override void BeforeDestroy()
    {
        Debug.Log("play sound blast");
        SoundManager.Instance.Play("main_gun_blast");
        TheGame.Instance.ControllerAttack.Attack(transform, false, AttackType.MainGunBlast, HitPosition, new Vector3(2, 2, 2),1, default);
        base.BeforeDestroy();

    }

}
