using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformUpdate
{
    public uint Tick { get; private set; }
    public Vector3 Position { get; private set; }

    public TransformUpdate(uint tick, Vector3 position)
    {
        Tick = tick;
        Position = position;
    }
}
