using System;
using UnityEngine;

[Serializable]
public class ChildSpriteConfiguration
{
    [SerializeField] private ChildStateType spriteType;
    [SerializeField] private Sprite sprite;

    public ChildStateType SpriteType => spriteType;

    public Sprite Sprite => sprite;
}