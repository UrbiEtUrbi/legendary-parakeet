using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerInstance : MonoBehaviour,IHealth
{
    public bool isAlive => true;

    public void ChangeHealth(float amount)
    {
        TheGame.Instance.Tower.ChangeHealth(amount);
    }

    public void ChangeHealth(float amount, AttackType type)
    {
        TheGame.Instance.Tower.ChangeHealth(amount, type);
    }

    public void Die()
    {
       
    }

    public void SetInitialHealth(float amount)
    {
       
    }

    
}
