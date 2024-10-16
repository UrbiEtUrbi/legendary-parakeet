using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(DestroyDelayed))]
public class AttackObject : MonoBehaviour
{
    [SerializeField]
    LayerMask TargetLayer;
    DestroyDelayed dd;

    bool initialized = false;

    Vector2 attackSize;
    float damage;

    [SerializeField]
    bool generateImpulse;
    [SerializeField]
    float impulseAmplitude;

    AttackType type;

   public UnityAction OnBeforeDestroy;

    [SerializeField]
    bool hitMultiple;
    [SerializeField]
    bool destroyOnHit;

    [SerializeField]
    float Force;

    [SerializeField]
    float Radius;

    [SerializeField]
    float Uplift;


    List<IHealth> HitTargets = new();

    private void Awake()
    {
        dd = GetComponent<DestroyDelayed>();
     
    }

    public void Init(Vector2 size, Vector3 position, float _damage, float lifetime, AttackType type)
    {

        if (lifetime > 0)
        {
            dd.Init(lifetime);
        }
        attackSize = size;
        damage = _damage;
        transform.position = position;
        initialized = true;
        this.type = type;
    }


    private void Update()
    {
        if (!initialized) {
            return;
        }

        var colliders = Physics2D.OverlapBoxAll(transform.position, attackSize, transform.rotation.eulerAngles.y, TargetLayer);

        foreach (var colliderHit in colliders)
        {
            if (colliderHit != null)
            {
               
                var h = colliderHit.GetComponent<IHealth>();
                if (h == null)
                {
                    continue;
                }
                if (hitMultiple && HitTargets.Contains(h)){
                    continue;
                }


                CancelInvoke();
                if (generateImpulse)
                {
                  //TODO shake camera
                }

                if (Force > 0)
                {
                    colliderHit.GetComponent<WalkingEnemy>().AddForce(transform.position,Force,Radius,Uplift );
                }
                colliderHit.GetComponent<IHealth>().ChangeHealth(-damage, type);

                var lp = GetComponent<Projectile>();
                if (lp != null)
                {
                    lp.BeforeDestroy();
                }

                if (!hitMultiple && destroyOnHit)
                {
                    Destroy(gameObject);
                    break;
                }
                else
                {
                    HitTargets.Add(h);

                }
            }
        }

        if (destroyOnHit && HitTargets.Count > 0)
        {
            Destroy(gameObject);
        }
    }



    private void OnDestroy()
    {
        if (OnBeforeDestroy != null)
        {
            OnBeforeDestroy.Invoke();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, attackSize);
    }



}
