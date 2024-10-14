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
        if (amount < 1 && amount > 0)
        {
            amount = MaxHp * amount;
        }
        if (amount < 0)
        {
            SoundManager.Instance.Play("tower_hit");
        }

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

    public void SetMaxHealth(float amount)
    {
        var dif = MaxHp - CurrentHP;
        MaxHp = amount;
        CurrentHP = MaxHp-dif;
    }

    public void Die()
    {

        SoundManager.Instance.Play("tower_dead");
        TheGame.Instance.OnGameOver();
    }

    public void SetInitialHealth(float amount)
    {
        MaxHp = amount;
        CurrentHP = amount;
    }
}
