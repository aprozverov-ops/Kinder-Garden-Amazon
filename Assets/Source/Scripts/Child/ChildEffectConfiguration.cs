using System;
using UnityEngine;

[Serializable]
public class ChildEffectConfiguration
{
    [SerializeField] private ChildEffectType childEffectType;
    [SerializeField] private GameObject effectObject;

    public ChildEffectType ChildEffectType => childEffectType;

    public GameObject EffectObject => effectObject;
}