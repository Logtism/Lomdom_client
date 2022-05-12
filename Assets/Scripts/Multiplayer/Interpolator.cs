using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interpolator : MonoBehaviour
{
    [SerializeField] private float TimeElapsed = 0f;
    [SerializeField] private float TimeToReachTarget = 0.1f;
    [SerializeField] private float MovementThreshold = 0.1f;

    private readonly List<TransformUpdate> futureTransformUpdates = new List<TransformUpdate>();
    private float SquareMovementThreshold;
    private TransformUpdate to;
    private TransformUpdate from;
    private TransformUpdate previous;

    private void Start()
    {
        SquareMovementThreshold = MovementThreshold * MovementThreshold;
        to = new TransformUpdate(NetworkManager.Singleton.ServerTick, transform.position);
        from = new TransformUpdate(NetworkManager.Singleton.InterpolationTick, transform.position);
        previous = new TransformUpdate(NetworkManager.Singleton.InterpolationTick, transform.position);

    }

    private void Update()
    {
        for (int i = 0; i < futureTransformUpdates.Count; i++)
        {
            if (NetworkManager.Singleton.ServerTick >= futureTransformUpdates[i].Tick);
            {
                previous = to;
                to = futureTransformUpdates[i];
                from = new TransformUpdate(NetworkManager.Singleton.InterpolationTick, transform.position);

                futureTransformUpdates.RemoveAt(i);
                i--;
                TimeElapsed = 0f;
                TimeToReachTarget = (to.Tick - from.Tick) * Time.fixedDeltaTime;
            }
        }
        TimeElapsed += Time.deltaTime;
        InterpolatePosition(TimeElapsed / TimeToReachTarget);
    }

    private void InterpolatePosition(float LerpAmount)
    {
        if ((to.Position - previous.Position).sqrMagnitude < SquareMovementThreshold)
        {
            if (to.Position != from.Position)
            {
                transform.position = Vector3.Lerp(from.Position, to.Position, LerpAmount);
            }
            return;
        }

        transform.position = Vector3.LerpUnclamped(from.Position, to.Position, LerpAmount);
    }

    public void NewUpdate(uint tick, Vector3 position)
    {
        if (tick <= NetworkManager.Singleton.InterpolationTick)
        {
            return;
        }
        for (int i = 0; i < futureTransformUpdates.Count; i++)
        {
            if (tick < futureTransformUpdates[i].Tick)
            {
                futureTransformUpdates.Insert(i, new TransformUpdate(tick, position));
            }
        }

        futureTransformUpdates.Add(new TransformUpdate(tick, position));
    }
}
