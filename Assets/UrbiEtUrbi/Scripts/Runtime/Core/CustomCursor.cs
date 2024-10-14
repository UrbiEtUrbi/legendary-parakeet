using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "IdleDuck/CustomCursor", fileName ="CustomCursorSettings")]
public class CustomCursor : ScriptableObject
{
    public Texture2D Idle;
    public Texture2D Tap;
    public Vector2 HotSpot;
}


