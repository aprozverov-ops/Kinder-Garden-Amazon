using System;
using UnityEngine;

[Serializable]
public class PointerConfiguration
{
    [SerializeField] private Sprite sprite;
    [SerializeField] private PointerType pointerType;

    public Sprite Sprite => sprite;

    public PointerType PointerType => pointerType;
}