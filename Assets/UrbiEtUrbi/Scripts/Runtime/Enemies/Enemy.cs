using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : PoolObject, IHealth
{



    [SerializeField]
    float Height;

    [SerializeField]
    EnemyDefinition Definition;


    float currentHealth;
    float maxHealth;

    float damage;
    protected float Speed;

    IHealth Target;

    [SerializeField]
    protected SpriteRenderer SpriteRenderer;

    protected Animator Animator;


    void Awake()
    {
        Animator = GetComponent<Animator>();
    }

    public void ChangeHealth(float amount)
    {

        currentHealth += amount;

        currentHealth = Mathf.Min(currentHealth, maxHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void ChangeHealth(float amount, AttackType type)
    {
        ChangeHealth(amount);
    }

    public  void Die()
    {
        BeforeDeath();
        PoolManager.Despawn(this);
    }

    public void SetInitialHealth(float amount)
    {

        maxHealth = amount;
        currentHealth = amount;
    }

    public virtual void Init(float speed, IHealth target, float TargetPos)
    {
        SetInitialHealth(Definition.MaxHealth);
        this.damage = Definition.Damage;
        Speed = speed;
        Target = target;
    }

    protected virtual void BeforeDeath()
    {

    }

    protected void Attack()
    {
        Animator.SetTrigger("attack");
        Target.ChangeHealth(-damage);
    }


}
