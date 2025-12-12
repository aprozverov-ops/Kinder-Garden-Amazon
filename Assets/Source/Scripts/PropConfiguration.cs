using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PropConfiguration
{
    [SerializeField] private PropType propType;
    [SerializeField] private List<Transform> spawnPositions;
    [SerializeField] private GameObject propPrefab;

    public PropType PropType => propType;

    public List<Transform> SpawnPositions => spawnPositions;

    public GameObject PropPrefab => propPrefab;
}