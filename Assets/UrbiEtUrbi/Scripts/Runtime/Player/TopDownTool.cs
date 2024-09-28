using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using PrimeTween;

public class TopDownTool : MonoBehaviour
{


    [SerializeField]
    SpriteRenderer SpriteRenderer;

    [SerializeField]
    float kickback = -0.2f;

    [SerializeField]
    float kickbackDuration = 0.2f;

    private void OnEnable()
    {
        if (ControllerInput.Instance != null)
        {
            ControllerInput.Instance.LeftClick.AddListener(OnUseTool);
        }
    }

    private void OnDisable()
    {

        if (ControllerInput.Instance != null)
        {
            ControllerInput.Instance.LeftClick.RemoveListener(OnUseTool);
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
    }

    protected virtual void Move(float angle)
    {


        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        SpriteRenderer.flipY = Mathf.Abs(angle) > 90f;

    }

    void OnUseTool(bool use)
    {
        if (use)
        {
            Tween.PunchLocalPosition(SpriteRenderer.transform, new Vector3(kickback, 0, 0), duration: kickbackDuration, cycles: 0, easeBetweenShakes: Ease.Linear, enableFalloff: true);
        }
    }
}
