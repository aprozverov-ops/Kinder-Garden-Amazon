using System;
using System.Threading;
using EventBusSystem;
using UnityEngine;

public enum EffectType
{
    Poof,
    LevelUp,
    Confity,
}

[Serializable]
public class EffectConfiguration
{
    [SerializeField] private EffectType effectType;
    [SerializeField] private TemporaryMonoPooled effect;

    public EffectType EffectType => effectType;

    public TemporaryMonoPooled Effect => effect;
}