using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : IHealth
{

    float MaxHp;
    float CurrentHP;

    public bool isAlive => CurrentHP > 0;

    public void ChangeHealth(float amount)
    {
        CurrentHP += amount;
        CurrentHP = Mathf.Min(CurrentHP, MaxHp);
        (TheGame.Instance.GameCycleManager.GetCurrentState as NightState).SetHealth(CurrentHP / MaxHp);
      //  Debug.Log($"{CurrentHP} {CurrentHP / MaxHp}");
        if (CurrentHP <= 0)
        {
            (TheGame.Instance.GameCycleManager.GetCurrentState as NightState).SetHealth(0);
            if (CurrentHP <= 0)
                Die();
        }
    }

    public void ChangeHealth(float amount, AttackType type)
    {
        ChangeHealth(amount);
    }

    public void Die()
    {

        TheGame.Instance.OnGameOver();
    }

    public void SetInitialHealth(float amount)
    {
        MaxHp = amount;
        CurrentHP = amount;
    }
}
