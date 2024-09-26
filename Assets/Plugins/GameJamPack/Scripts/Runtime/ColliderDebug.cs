using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderDebug : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    TMPro.TMP_Text label;
    void Start()
    {
        var objs=  FindObjectsByType<Collider>(sortMode: FindObjectsSortMode.None);
        var str = string.Empty;

        foreach (var o in objs)
        {
            str += $"{o.gameObject.name} {(o.material != null ? o.material.name : "null material")}" +
                $" {(o.material != null ? o.material.dynamicFriction : "null")}" +
                $" {(o.material != null ? o.material.staticFriction : "null")}" +
                $" {(o.material != null ? o.material.frictionCombine : "null")}\n";
        }
        label.text = str;
    }

  
}
