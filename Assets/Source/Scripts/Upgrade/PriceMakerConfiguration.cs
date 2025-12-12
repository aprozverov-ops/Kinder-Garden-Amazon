using System;
using UnityEngine;

[Serializable]
public class PriceMakerConfiguration
{
    [SerializeField] private int startPrice;
    [SerializeField] private float addPercentPerLevel;
    [SerializeField] private int maxLevel;

    public int MAXLevel => maxLevel;

    public int StartPrice => startPrice;

    public float AddPercentPerLevel => addPercentPerLevel;
}