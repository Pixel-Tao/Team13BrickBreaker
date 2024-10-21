using UnityEngine;

public class Util
{
    public static LayerMask CombineLayersToMask(params Defines.ELayerMask[] layers)
    {
        LayerMask mask = 0;
        foreach (var layer in layers)
            mask |= 1 << (int)layer;
        return mask;
    }
}
