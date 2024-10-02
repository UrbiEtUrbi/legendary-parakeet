using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerChanger : MonoBehaviour
{
    [SerializeField]
    int InputLayer;

    [SerializeField]
    int GoToLayer;

    Map Map;

    private void Awake()
    {
        Map = FindObjectOfType<Map>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (Map.CurrentLayer == InputLayer)
            {
                Map.EnterLayer(GoToLayer);
            }
        }
    }

    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        UnityEditor.Handles.Label(transform.position, $"{InputLayer} -> {GoToLayer}");
#endif
    }
}
