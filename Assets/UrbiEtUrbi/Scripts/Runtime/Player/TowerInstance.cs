using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerInstance : MonoBehaviour,IHealth
{

    [SerializeField]
    NodeData TowerArmor;

    public bool isAlive => true;

    public void ChangeHealth(float amount)
    {
        amount = Mathf.Max(0, amount*TowerArmor.GetValue());
        TheGame.Instance.Tower.ChangeHealth(amount);
    }

    public void ChangeHealth(float amount, AttackType type)
    {

        amount = Mathf.Max(0, amount*TowerArmor.GetValue());
        TheGame.Instance.Tower.ChangeHealth(amount, type);
    }

    public void Die()
    {
       
    }

    public void SetInitialHealth(float amount)
    {
       
    }

    
}
