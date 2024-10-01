using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingEnemy : Enemy
{




    


    [SerializeField]
    float StopDistance;

    [SerializeField]
    float AttackRate;



    float attackTimer;

    float TargetPos;
    int direction;

    float force = default;
    Vector3 origin = default;
    float radius;
    float uplift;

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
    }

    protected virtual void FixedUpdate()
    {

        //  Debug.Log($"{direction} {TargetPos} {direction*TargetPos} {transform.position.x}");
        if (direction == Mathf.Sign(TargetPos - transform.position.x))
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
        if (attackTimer <= 0)
        {
            attackTimer = 1f / AttackRate;
            Attack();
        }
    }

    public override void Init(int health, int damage, float speed, IHealth target, float TargetPos)
    {

        this.TargetPos = TargetPos;
        direction = TargetPos > transform.position.x ? 1 : -1;
        SpriteRenderer.flipX = direction == -1;
        base.Init(health, damage, speed, target, TargetPos);
    }

    protected override void  BeforeDeath()
    {
      
        var enemy = Instantiate<WalkingEnemy>(this).gameObject;
        Destroy(enemy.GetComponent<Enemy>());
        var rb = enemy.GetComponent<Rigidbody2D>();
        rb.gravityScale = 1;
        rb.freezeRotation = false;
        if (force > 0)
        {
            rb.AddExplosionForce(force, origin, radius, uplift);
        }
        force = 0;
        enemy.AddComponent<DestroyDelayed>().Init(1f);
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
        Debug.Log($"{body.transform.position} {explosionPosition}");
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
            Debug.Log($"{dir} {wearoff} {baseForce} {upliftForce} {-baseForce.magnitude}");
            body.AddTorque(0.1f*-baseForce.magnitude, ForceMode2D.Impulse);
        }
        else
        {

            Debug.Log($"{dir} {wearoff} {baseForce} {upliftForce} {baseForce.magnitude}");
            body.AddTorque(0.1f*baseForce.magnitude, ForceMode2D.Impulse);
        }
    }
}