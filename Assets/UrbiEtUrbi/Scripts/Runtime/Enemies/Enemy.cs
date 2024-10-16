using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : PoolObject, IHealth
{



    [SerializeField]
    protected float Height;

    [SerializeField]
    protected EnemyDefinition Definition;




    float currentHealth;
    float maxHealth;

    float damage;
    protected float Speed;

    IHealth Target;

    [SerializeField]
    protected SpriteRenderer SpriteRenderer;

    protected Animator Animator;

    public bool isAlive => currentHealth > 0;

    public void SetLayer(int layer)
    {
        SpriteRenderer.sortingOrder = layer;
    }

    public int GetLayer()
    {
        return SpriteRenderer.sortingOrder;
    }

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

    public virtual void ChangeHealth(float amount, AttackType type)
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

    public virtual void Init(float speed, IHealth target, Transform TargetPos)
    {
        SetInitialHealth(Definition.MaxHealth);
        this.damage = Definition.Damage;
        Speed = Definition.Speed;
        Target = target;
    }

    protected virtual void BeforeDeath()
    {

    }

    protected virtual void Attack()
    {
        //Debug.Log("Attack");

        Animator.SetTrigger("attack");
        Target.ChangeHealth(-damage);
    }


}
