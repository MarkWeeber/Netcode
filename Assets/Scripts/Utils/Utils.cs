using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public static class Utils
{
    public static bool CheckLayer(LayerMask layerMask, int layer)
    {
        if ((layerMask.value & (1 << layer)) > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
