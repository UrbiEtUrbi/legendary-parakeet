using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AutoGun : TopDownTool, IMagazine
{





    [SerializeField]
    bool left;

    Enemy currentTarget;

    [SerializeField]
    NodeData FireRate;

    Magazine Magazine;
    List<TMP_Text> labels;

    public void AssignMagazine(Magazine magazine, List<TMP_Text> Labels)
    {
        Damage = 1f;
        Magazine = magazine;

        labels = Labels;
        
    }

    void UpdateLabels()
    {
        foreach (var label in labels)
        {
            label.text = $"{Magazine.Current}/{Magazine.Max}";
        }
    }

    protected override void Update()
    {
        if (currentTarget == null || !currentTarget.isAlive)
        {
            var enemies = (TheGame.Instance.GameCycleManager.GetCurrentState as NightState).Enemies;

            Enemy target = null;
            float dist = 1000;
            foreach (var enemy in enemies)
            {
                if (left && enemy.transform.position.x < 0)
                {
                    if (Mathf.Abs(enemy.transform.position.x) < dist)
                    {
                        target = enemy;
                        dist = Mathf.Abs(enemy.transform.position.x);
                    }
                }
                else if(!left && enemy.transform.position.x >= 0)
                {
                    if (Mathf.Abs(enemy.transform.position.x) < dist)
                    {
                        target = enemy;
                        dist = Mathf.Abs(enemy.transform.position.x);
                    }
                }
            }
            if (target != null)
            {
                ExternalTarget = target.transform;
                currentTarget = target;

            }
            else
            {
                ExternalTarget = null;
                currentTarget = null;
            }

        }

        
        base.Update();
        if (CanShoot && currentTarget != null && Magazine.Current > 0)
        {
            Magazine.Current--;
            ReloadTime =    1 / FireRate.GetValue();
            UpdateLabels();
            OnUseTool(true);
        }
    }

}
