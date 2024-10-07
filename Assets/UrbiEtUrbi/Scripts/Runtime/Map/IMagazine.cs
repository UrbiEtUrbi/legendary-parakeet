using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMagazine 
{

    Transform transform { get; }
    void AssignMagazine(Magazine magazine);
}
