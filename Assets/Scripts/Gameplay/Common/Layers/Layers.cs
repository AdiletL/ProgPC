using UnityEngine;

public static class Layers
{
    public readonly static LayerMask EVERYTHING = ~0;
    public readonly static LayerMask PLAYER = 1 << 3;
    public readonly static LayerMask BUILDABLE_OBJECT = 1 << 8;
}
