using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingEnemy : Enemy
{




    


    [SerializeField]
    protected float StopDistance;

    [SerializeField]
    float AttackRate;



    float attackTimer;

    Transform TargetPos;
    int direction;

    float force = default;
    Vector3 origin = default;
    float radius;
    float uplift;

    bool blasted;
    bool hitGround;

    float blastedTimer;
    float blastedTime = 1f;

    [SerializeField]
    bool CanBeBlasted;

    public void AddForce(Vector3 origin, float force, float radius, float uplift)
    {
        this.force = force;
        this.origin = origin;
        this.radius = radius;
        this.uplift = uplift;
    }

    protected virtual void Update()
    {
        attackTimer -= Time.deltaTime;
        blastedTimer -= Time.deltaTime;
    }

    protected virtual void FixedUpdate()
    {


        if (blasted)
        {
            if (blastedTimer <= 0 && hitGround)
            {
                Animator.SetTrigger("run");
                var rb = GetComponent<Rigidbody2D>();
                rb.velocity = default;
                rb.gravityScale = 0;
                rb.SetRotation(0);
                rb.freezeRotation = true;
                blasted = false;
                rb.position = new Vector3(transform.position.x, Height, transform.position.z);
            }
            return;
        }

        if (Mathf.Abs(transform.position.x- TargetPos.position.x) >= StopDistance)
        {
            transform.position += new Vector3(Speed * direction * Time.deltaTime, 0, 0);
        }
        else
        {
            TryToAttack();
        }
    }

    void TryToAttack()
    {
        if (attackTimer <= 0 && !blasted)
        {
            attackTimer = 1f / AttackRate;
            Attack();
        }
    }

    public override void ChangeHealth(float amount, AttackType type)
    {
        base.ChangeHealth(amount, type);

        if (type == AttackType.MainGunBlast && CanBeBlasted)
        {
            var rb = GetComponent<Rigidbody2D>();
            rb.gravityScale = 1;
            rb.freezeRotation = false;
            if (force > 0)
            {
                blastedTimer = blastedTime;
                blasted = true;
                Animator.SetTrigger("fall");
                rb.AddExplosionForce(force, origin, radius, uplift);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.name == "Ground" && blasted)
        {
            hitGround = true;
        }
    }

    public override void Init( float speed, IHealth target, Transform Target)
    {

        this.TargetPos = Target;
        if (transform.position.x > Target.position.x + StopDistance)
        {
            direction = -1;
        }
        else if(transform.position.x < Target.position.x - StopDistance)
        {
            direction = 1;
        }
        transform.position = new Vector3(transform.position.x, Height, transform.position.z);
        SpriteRenderer.flipX = direction == -1;
        base.Init( speed, target, Target);
    }

    protected override void  BeforeDeath()
    {
        if (CanBeBlasted)
        {
            var enemy = Instantiate<WalkingEnemy>(this).gameObject;
            Destroy(enemy.GetComponent<Enemy>());
            var rb = enemy.GetComponent<Rigidbody2D>();
            rb.gravityScale = 1;
            rb.freezeRotation = false;
            if (force > 0)
            {
                blasted = true;
                enemy.GetComponent<Animator>().SetTrigger("fall");
                rb.AddExplosionForce(force, origin, radius, uplift);
            }
            force = 0;
            enemy.AddComponent<DestroyDelayed>().Init(1f);
        }
        (TheGame.Instance.GameCycleManager.GetCurrentState as NightState).RemoveEnemy();

    }

    


}
public static class Rigidbody2DExtension
{
    public static void AddExplosionForce(this Rigidbody2D body, float explosionForce, Vector3 explosionPosition, float explosionRadius)
    {
        var dir = (body.transform.position - explosionPosition);
        float wearoff = 1 - (dir.magnitude / explosionRadius);
        body.AddForce(dir.normalized * explosionForce * wearoff);
    }

    public static void AddExplosionForce(this Rigidbody2D body, float explosionForce, Vector3 explosionPosition, float explosionRadius, float upliftModifier)
    {
  //      Debug.Log($"{body.transform.position} {explosionPosition}");
        var dir = (body.transform.position - explosionPosition);
        dir = new Vector3(dir.x, dir.y, 0);
        float wearoff = 1 - (dir.magnitude / explosionRadius);
        Vector3 baseForce = dir.normalized * explosionForce * wearoff;
        body.AddForce(baseForce, ForceMode2D.Impulse);

        float upliftWearoff = 1 - upliftModifier / explosionRadius;
        Vector3 upliftForce = Vector2.up * explosionForce * upliftWearoff;
        body.AddForce(upliftForce, ForceMode2D.Impulse);


        if (body.position.x > explosionPosition.x)
        {
//            Debug.Log($"{dir} {wearoff} {baseForce} {upliftForce} {-baseForce.magnitude}");
            body.AddTorque(0.1f*-baseForce.magnitude, ForceMode2D.Impulse);
        }
        else
        {

  //          Debug.Log($"{dir} {wearoff} {baseForce} {upliftForce} {baseForce.magnitude}");
            body.AddTorque(0.1f*baseForce.magnitude, ForceMode2D.Impulse);
        }
    }
}