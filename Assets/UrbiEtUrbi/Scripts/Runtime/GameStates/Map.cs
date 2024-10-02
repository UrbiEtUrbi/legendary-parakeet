using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map : MonoBehaviour
{

  
    public PolygonCollider2D Confiner;

    [SerializeField]
    GameObject[] Walls;

    int currentLayer = -1;
    public int CurrentLayer => currentLayer;

    [SerializeField]
    TilemapRenderer foreground;


    private void Start()
    {
        EnterLayer(0);
    }

    public void EnterLayer(int layer)
    {
        for (int i = 0; i < Walls.Length; i++)
        {
            Walls[i].gameObject.SetActive(layer == i);
        }
        currentLayer = layer;
        if (currentLayer == 0)
        {
            foreground.sortingOrder = TheGame.Instance.GameCycleManager.GetCurrentState.Player.Art.sortingOrder - 1;

        }
        if (currentLayer == 1)
        {
            foreground.sortingOrder = TheGame.Instance.GameCycleManager.GetCurrentState.Player.Art.sortingOrder + 1;

        }
    }
}
