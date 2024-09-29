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
    float ReloadTime;

    [SerializeField]
    SpriteRenderer SpriteRenderer;

    [SerializeField]
    Transform Muzzle;

    [SerializeField]
    float kickback = -0.2f;

    [SerializeField]
    float kickbackDuration = 0.2f;

    float reloadTimer = 0;

    bool holdingUse = false;

    private void OnEnable()
    {
        if (ControllerInput.Instance != null)
        {
            ControllerInput.Instance.LeftClick.AddListener(OnUseTool);
        }

        if (TheGame.Instance != null)
        {
            TheGame.Instance.GameCycleManager.OnChangeState.AddListener(OnStateChanged);
        }
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

    private void Update()
    {
       
        var pos = TheGame.Instance.GameCycleManager.CurrentCamera.ScreenToWorldPoint(Mouse.current.position.value);
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

    void OnUseTool(bool use)
    {
     //   holdingUse = use;
        if (use && reloadTimer <= 0)
        {
            reloadTimer = ReloadTime;
            Tween.PunchLocalPosition(SpriteRenderer.transform, new Vector3(kickback, 0, 0), duration: kickbackDuration, cycles: 0, easeBetweenShakes: Ease.Linear, enableFalloff: true);

            var obj = TheGame.Instance.ControllerAttack.Attack(transform, false, Attack, Muzzle.position, Vector3.one, 1, -Muzzle.up);
            obj.OnBeforeDestroy =() =>  attackObjects.Remove(obj);
            attackObjects.Add(obj);
        }
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
