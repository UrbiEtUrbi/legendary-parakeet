using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class Map : MonoBehaviour
{

  
    public PolygonCollider2D Confiner;

    [SerializeField]
    GameObject[] Walls;

    int currentLayer = -1;
    public int CurrentLayer => currentLayer;

    [SerializeField]
    TilemapRenderer foreground;


    [SerializeField]
    Sprite DefaultCacheSprite;

    [SerializeField]
    Sprite DefaultSurvivorSprite;

    List<ResourcePickup> Pickups = new();
    List<bool> pickedUp = new();

    public bool HasPickedUpAll => pickedUp.All(x => x);

    private void Start()
    {
        EnterLayer(0);

      
        var objects = FindObjectsByType<MapObjectPlaceholder>(FindObjectsSortMode.None);
        foreach (var o in objects)
        {
            var cache = GetCache(o.Drops);
            if (cache == null)
            {
                continue;
            }
            var pickup = PoolManager.Spawn<ResourcePickup>("ResourcePickup", transform, o.transform.position);
            pickup.Init(cache, o.IsSurvivor ? DefaultSurvivorSprite : DefaultCacheSprite);
            Pickups.Add(pickup);
            pickedUp.Add(false);
        }
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

    public void PickedUp(Pickup pickup)
    {

        var idx = Pickups.IndexOf(pickup as ResourcePickup);
        if (idx != -1)
        {
            pickedUp[idx] = true;
        }
    }


    Cache GetCache(List<CacheDrop> cacheDrops)
    {
        int weightSum = cacheDrops.Sum(x => x.Weight);

        int currentWeight = 0;

        int targetWeight = Random.Range(0, weightSum);

        for (int i = 0; i < cacheDrops.Count; i++)
        {


            if (targetWeight >= currentWeight && targetWeight < currentWeight + cacheDrops[i].Weight)
            {
                return cacheDrops[i].Cache;
            }
            currentWeight += cacheDrops[i].Weight;
        }
        return cacheDrops[^1].Cache;




    }

    public void Clear()
    {
        for (int i = Pickups.Count - 1; i >= 0; i--)
        {
            PoolManager.Despawn(Pickups[i]);
        }
        Pickups.Clear();
    }
}


[System.Serializable]
public class CacheDrop
{
    public Cache Cache;
    public int Weight;

}
