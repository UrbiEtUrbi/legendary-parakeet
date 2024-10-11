using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName ="Nightfall/Cache", fileName ="Cache")]
public class Cache : ScriptableObject
{

    public Sprite OverrideSprite;
    public List<Content> Contents;

    public List<ResourceAmount> Open()
    {
        var gain = new List<ResourceAmount>();

        foreach (var c in Contents)
        {
            gain.Add(new ResourceAmount
            (
                c.Resource.ID,
                Random.Range(c.Min, c.Max + 1)
            ));
        }
        return gain;

    }
}

[System.Serializable]
public class Content
{
    public Resource Resource;
    public int Min;
    public int Max;

}
