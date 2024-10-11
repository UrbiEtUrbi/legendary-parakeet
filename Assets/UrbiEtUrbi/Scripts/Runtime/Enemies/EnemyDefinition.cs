using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Nightfall/EnemyDef", fileName ="enemyDef")]
public class EnemyDefinition : ScriptableObject
{

    [SerializeField]
    float BaseDamage;

    [SerializeField]
    float DamagePerRound;

    public float Damage => BaseDamage + DamagePerRound * TheGame.Instance.RoundNumber;

    [SerializeField]
    float BaseHealth;

    [SerializeField]
    float HealthPerRound;

    public float MaxHealth => BaseHealth + HealthPerRound * TheGame.Instance.RoundNumber;

    [SerializeField]
    float BaseAttackRate;

    [SerializeField]
    float AttackRatePerRound;

    public float AttackRate => BaseAttackRate + AttackRatePerRound * TheGame.Instance.RoundNumber;
}
