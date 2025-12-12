using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ChildPlayConfiguration
{
    [SerializeField] private List<GameObject> playItem;
    [SerializeField] private Transform position;
    [SerializeField] private ChildAnimationType childAnimationType;
    [SerializeField] private float minPlayTime;
    [SerializeField] private float maxPlayTime;

    public bool IsActivate = true;

    public List<GameObject> PlayItem => playItem;

    public Transform Position => position;

    public ChildAnimationType ChildAnimationType => childAnimationType;

    public float MINPlayTime => minPlayTime;

    public float MAXPlayTime => maxPlayTime;
}