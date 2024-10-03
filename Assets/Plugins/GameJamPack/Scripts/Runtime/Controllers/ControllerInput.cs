using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class ControllerInput : GenericSingleton<ControllerInput>
{
    [HideInInspector]
    public UnityEvent<float> Horizontal = new UnityEvent<float>();
    [HideInInspector]
    public UnityEvent<float> Vertical = new UnityEvent<float>();

    [HideInInspector]
    public UnityEvent<bool> Interact = new UnityEvent<bool>();

    [HideInInspector]
    public UnityEvent<bool> LeftClick = new UnityEvent<bool>();





    void OnHorizontal(InputValue inputValue)
    {
        
        var horizontalInputRaw = inputValue.Get<float>();
        Horizontal.Invoke(horizontalInputRaw);
        

    }

    void OnVertical(InputValue inputValue)
    {
       
        var vertInputRaw = inputValue.Get<float>();
        Vertical.Invoke(vertInputRaw);
        

    }

    void OnInteract(InputValue inputValue)
    {

        var isPressed = inputValue.Get<float>();
        Interact.Invoke(isPressed > 0);
    }

    void OnLeftClick(InputValue inputValue)
    {
        var isPressed = inputValue.Get<float>();

        LeftClick.Invoke(isPressed > 0);
    }

    
   
}
