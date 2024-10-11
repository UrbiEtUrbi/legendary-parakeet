using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemy : WalkingEnemy
{

    [SerializeField]
    List<TopDownTool> TopDownTool;

    List<float> timers = new();

    


    public override void Init(float speed, IHealth target, Transform Target)
    {
        StopDistance *= Random.Range(0.9f, 1.1f);
        base.Init(speed, target, Target);
        foreach (var tdt in TopDownTool)
        {
            timers.Add((1 / (float)AttackRate) * Random.Range(0.9f, 1.1f));
            tdt.Init(Definition.Damage, 1f / Definition.AttackRate, Target);
            tdt.transform.localPosition = new Vector3(direction * tdt.transform.localPosition.x, tdt.transform.localPosition.y, tdt.transform.localPosition.z);
        }
        transform.position = new Vector3(transform.position.x, Height+Random.Range(-0.2f,0.2f), transform.position.z);



    }

    protected override void TryToAttack()
    {
        for(int i = 0; i < TopDownTool.Count;i++)
        {
            var tdt = TopDownTool[i];
            timers[i] -= Time.deltaTime;
            if (timers[i] <= 0)
            {
                timers [i]= (1 / (float)AttackRate) * Random.Range(0.9f,1.1f);
                tdt.OnUseTool(true);
            }
        }
    }

    protected override void Attack()
    {

       
    }
}
