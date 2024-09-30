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

    Vector2 force = default;

    public void AddForce(Vector2 force)
    {
        this.force = force;
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
        Debug.Log($"force {force}");
        rb.AddForceAtPosition(force, rb.position-new Vector2(0,-0.2f));
        
        enemy.AddComponent<DestroyDelayed>().Init(1f);
        (TheGame.Instance.GameCycleManager.GetCurrentState as NightState).RemoveEnemy();

    }

    


}
