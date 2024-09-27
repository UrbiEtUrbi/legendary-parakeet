using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerResources : MonoBehaviour
{
    public List<Resource> ResourceCollection;

    public Resource GetResourceByID(int id)
    {
        var idx = ResourceCollection.FindIndex(x => x.ID == id);
        if (idx != -1)
        {
            return ResourceCollection[idx];
        }
        else
        {
            return null;
        }
    }
}
