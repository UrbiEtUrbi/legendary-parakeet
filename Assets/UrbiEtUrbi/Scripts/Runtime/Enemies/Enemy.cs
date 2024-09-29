using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : PoolObject, IHealth
{


    int currentHealth;
    int maxHealth;

    int damage;
    protected float Speed;

    IHealth Target;

    [SerializeField]
    protected SpriteRenderer SpriteRenderer;


    public void ChangeHealth(int amount)
    {

        currentHealth += amount;

        currentHealth = Mathf.Min(currentHealth, maxHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void ChangeHealth(int amount, AttackType type)
    {
        ChangeHealth(amount);
    }

    public  void Die()
    {
        BeforeDeath();
        PoolManager.Despawn(this);
    }

    public void SetInitialHealth(int amount)
    {

        maxHealth = amount;
        currentHealth = amount;
    }

    public virtual void Init(int health, int damage, float speed, IHealth target, float TargetPos)
    {
        SetInitialHealth(health);
        this.damage = damage;
        Speed = speed;
        Target = target;
    }

    protected virtual void BeforeDeath()
    {

    }

    protected void Attack()
    {
        Target.ChangeHealth(-damage);
    }


}
