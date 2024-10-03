using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObjectPlaceholder : MonoBehaviour
{
    public bool IsSurvivor;

    [BeginGroup, EndGroup, SerializeField]
    public List<CacheDrop> Drops;
}
