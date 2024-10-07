using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public interface IMagazine 
{

    Transform transform { get; }
    void AssignMagazine(Magazine magazine, List<TMP_Text> labels);
}
