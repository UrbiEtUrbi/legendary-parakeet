using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName ="Nightfall/Resource", fileName ="resource")]
public class Resource : ScriptableObject
{
    public int ID;
    public string Name;
    public Sprite Icon;
}
