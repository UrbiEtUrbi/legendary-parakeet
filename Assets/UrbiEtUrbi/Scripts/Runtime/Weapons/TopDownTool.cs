using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using PrimeTween;

public class TopDownTool : MonoBehaviour
{


    [SerializeField]
    AttackType Attack;

    [SerializeField]
    protected float ReloadTime;

    [SerializeField]
    SpriteRenderer SpriteRenderer;

    [SerializeField]
    Transform Muzzle;

    [SerializeField]
    float kickback = -0.2f;

    [SerializeField]
    float kickbackDuration = 0.2f;

    float reloadTimer = 0;

    protected virtual bool CanShoot => reloadTimer <= 0;

    [SerializeField]
    bool UseAnimation;

    Animator Animator;

    bool holdingUse = false;

    [SerializeField]
    bool PlayerControlled;

    public Transform ExternalTarget;

    protected float Damage;

    private void OnEnable()
    {
        Animator = GetComponent<Animator>();
        if (ControllerInput.Instance != null && PlayerControlled)
        {
            ControllerInput.Instance.LeftClick.AddListener(OnUseTool);
        }

        if (TheGame.Instance != null)
        {
            TheGame.Instance.GameCycleManager.OnChangeState.AddListener(OnStateChanged);
        }
    }


    public void Init(float Damage, float reloadTime, Transform target)
    {
        this.Damage = Damage;
        this.ReloadTime = reloadTime;
        ExternalTarget = target;

    }
    private void OnDisable()
    {

        if (ControllerInput.Instance != null)
        {
            ControllerInput.Instance.LeftClick.RemoveListener(OnUseTool);
        }

        if (TheGame.Instance != null)
        {
            TheGame.Instance.GameCycleManager.OnChangeState.RemoveListener(OnStateChanged);
        }
    }

    protected virtual void Update()
    {


        Vector3 pos = default;
        if (ExternalTarget == null && PlayerControlled)
        {

            pos = TheGame.Instance.GameCycleManager.CurrentCamera.ScreenToWorldPoint(Mouse.current.position.value);
        }
        else if (ExternalTarget != null)
        {

            pos = ExternalTarget.transform.position;
        }
        else
        {
            reloadTimer -= Time.deltaTime;
            return;
        }

            Vector3 direction = pos - transform.position;
            direction.z = 0;  // Ignore z-axis since it's 2D

            // Calculate the angle between the direction vector and the world up vector (0, 1, 0)
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;


        

       

        // Apply the rotation to the sprite
        Move(angle);

        reloadTimer -= Time.deltaTime;
        if (holdingUse)
        {
         //   OnUseTool(holdingUse);
        }

    }

    protected virtual void Move(float angle)
    {


        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));    
        SpriteRenderer.flipY = Mathf.Abs(angle) > 90f;

    }

    List<AttackObject> attackObjects = new List<AttackObject>();

    public void OnUseTool(bool use)
    {
     //   holdingUse = use;
        if (use && CanShoot)
        {
            if (!UseAnimation)
            {
                Tween.PunchLocalPosition(SpriteRenderer.transform, new Vector3(kickback, 0, 0), duration: kickbackDuration, cycles: 0, easeBetweenShakes: Ease.Linear, enableFalloff: true);
            }
            else
            {
                Animator.SetTrigger("shoot");
            }
            reloadTimer = ReloadTime;

          

            var obj = TheGame.Instance.ControllerAttack.Attack(transform, false, Attack, Muzzle.position, Vector3.one, Damage, -Muzzle.up);
            obj.OnBeforeDestroy =() =>  attackObjects.Remove(obj);
            attackObjects.Add(obj);
            Use();
        }
    }

    protected virtual void Use()
    {

    }

    void OnStateChanged(GameStateType gameStateType)
    {
        Cleanup();
    }

    public void Cleanup()
    {
        for (int i = attackObjects.Count - 1; i >= 0; i--)
        {
            if (attackObjects[i] != null)
            {
                Destroy(attackObjects[i].gameObject);
            }
        }

        attackObjects.Clear();
    }
}
