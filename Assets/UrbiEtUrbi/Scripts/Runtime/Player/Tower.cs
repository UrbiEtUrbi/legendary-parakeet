using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : IHealth
{

    float MaxHp;
    float CurrentHP;

    public void ChangeHealth(float amount)
    {
        CurrentHP += amount;
        CurrentHP = Mathf.Min(CurrentHP, MaxHp);
        if (CurrentHP <= 0)
        {
            Die();
        }
    }

    public void ChangeHealth(float amount, AttackType type)
    {
        
    }

    public void Die()
    {
        //Game Over
//        Debug.Log($"your tower was destroyed");
    }

    public void SetInitialHealth(float amount)
    {
        MaxHp = amount;
        CurrentHP = amount;
    }
}
