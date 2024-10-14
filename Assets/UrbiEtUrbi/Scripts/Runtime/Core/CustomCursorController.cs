using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class CustomCursorController : MonoBehaviour
{


    [SerializeField]
    CustomCursor CustomCursor;

    [SerializeField]
    bool DisableCustomCursor;

    private void Awake()
    {

#if !UNITY_EDITOR
        Destroy(gameObject);    
#endif

        if (CustomCursor == null || DisableCustomCursor)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }


    }



    private void Start()
    {
        Cursor.SetCursor(CustomCursor.Idle, CustomCursor.HotSpot, CursorMode.ForceSoftware);
    }

    private void Update()
    {


#if ENABLE_INPUT_SYSTEM
        if (Mouse.current.leftButton.isPressed)
        {
            Cursor.SetCursor(CustomCursor.Tap, CustomCursor.HotSpot, CursorMode.ForceSoftware);

        }
        else
        {
            Cursor.SetCursor(CustomCursor.Idle, CustomCursor.HotSpot, CursorMode.ForceSoftware);
        }
#elif ENABLE_LEGACY_INPUT_MANAGER
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.SetCursor(CustomCursor.Tap, CustomCursor.HotSpot, CursorMode.ForceSoftware);
        }
        else if (Input.GetMouseButtonUp(0))
        {

            Cursor.SetCursor(CustomCursor.Idle, CustomCursor.HotSpot, CursorMode.ForceSoftware);
        }
#endif
    }
}
