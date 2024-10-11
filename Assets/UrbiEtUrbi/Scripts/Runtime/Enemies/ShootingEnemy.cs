using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemy : WalkingEnemy
{

    [SerializeField]
    TopDownTool TopDownTool;

    public override void Init(float speed, IHealth target, Transform Target)
    {
        StopDistance *= Random.Range(0.9f, 1.1f);
        base.Init(speed, target, Target);
        TopDownTool.Init(Definition.Damage, 1f/Definition.AttackRate, Target);
        
    }

    protected override void Attack()
    {
        TopDownTool.OnUseTool(true);
    }
}
