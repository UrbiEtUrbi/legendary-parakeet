using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth 
{
    public void SetInitialHealth(float amount);
    public void ChangeHealth(float amount);
    public void ChangeHealth(float amount, AttackType type);
    public void Die();
    public bool isAlive { get; }
}
