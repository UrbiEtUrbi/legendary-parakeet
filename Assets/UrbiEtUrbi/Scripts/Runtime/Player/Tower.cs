using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : IHealth
{

    int MaxHp;
    int CurrentHP;

    public void ChangeHealth(int amount)
    {
        CurrentHP += amount;
        CurrentHP = Mathf.Min(CurrentHP, MaxHp);
        if (CurrentHP <= 0)
        {
            Die();
        }
    }

    public void ChangeHealth(int amount, AttackType type)
    {
        
    }

    public void Die()
    {
        //Game Over
//        Debug.Log($"your tower was destroyed");
    }

    public void SetInitialHealth(int amount)
    {
        MaxHp = amount;
        CurrentHP = amount;
    }
}
