using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LayerMaskExtention 
{
    public static bool IsInLayerMasks(this GameObject gameObject,int layerMasks)
    {
        return (layerMasks == (layerMasks | (1 << gameObject.layer)));
    }
}
