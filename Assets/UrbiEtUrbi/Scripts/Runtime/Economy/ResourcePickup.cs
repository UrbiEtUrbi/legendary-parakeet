using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ResourcePickup : Pickup
{
    [HideInInspector]
    public Cache cache;



    [SerializeField]
    SpriteRenderer SpriteRenderer;

    public void Init(Cache cache, Sprite defaultSprite)
    {


       this.cache = cache;
       SpriteRenderer.sprite = cache.OverrideSprite == null ? defaultSprite : cache.OverrideSprite;
       base.Init();
    }
}
