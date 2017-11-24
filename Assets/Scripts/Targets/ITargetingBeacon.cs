using UnityEngine;
using System;

namespace Targets
{
    public interface ITargetingBeacon
    {
        TargetType TargetType { get; }
        Vector3 Position { get; }
        bool IsValid { get; }

        event Action<ITargetingBeacon> TargetRemovedEvent;
    }
}
