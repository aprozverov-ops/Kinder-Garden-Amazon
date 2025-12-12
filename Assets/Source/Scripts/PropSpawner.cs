using System;
using System.Collections;
using System.Collections.Generic;
using Kuhpik;
using UnityEngine;
using Random = UnityEngine.Random;

public class PropSpawner : GameSystem
{
    [SerializeField] private int amountSpawnProp;
    [SerializeField] private List<PropConfiguration> propConfigurations;

    private readonly Dictionary<PropType, List<Prop>> m_spawnedProp = new Dictionary<PropType, List<Prop>>();

    public override void OnInit()
    {
        Initialize();
        StartCoroutine(SpawnPropCoroutine());
    }

    private void Initialize()
    {
        StartCoroutine(InitializeTutor());
    }

    private IEnumerator InitializeTutor()
    {
        foreach (PropType propType in Enum.GetValues(typeof(PropType)))
        {
            m_spawnedProp.Add(propType, new List<Prop>());
        }

        while (player.IsTutorialFinish == false)
        {
            yield return new WaitForSeconds(1f);
        }

        foreach (var spawnProp in m_spawnedProp)
        {
            for (int i = 0; i < amountSpawnProp; i++)
            {
                SpawnProp(spawnProp.Key);
            }
        }
    }

    private IEnumerator SpawnPropCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if (player.IsTutorialFinish == false) continue;
            foreach (var spawnedProp in m_spawnedProp)
            {
                CheckProp(spawnedProp.Key);
            }
        }
    }

    private void CheckProp(PropType propType)
    {
        foreach (var prop in m_spawnedProp[propType])
        {
            if (prop.IsAttach)
            {
                m_spawnedProp[propType].Remove(prop);
                SpawnProp(propType);
                CheckProp(propType);
                break;
            }
        }
    }

    public void SpawnProp(PropType propType)
    {
        foreach (var propConfiguration in propConfigurations)
        {
            if (propConfiguration.PropType == propType)
            {
                var pos = propConfiguration.SpawnPositions[Random.Range(0, propConfiguration.SpawnPositions.Count)];
                var newItem = GameObject.Instantiate(propConfiguration.PropPrefab);
                newItem.transform.position = pos.position;
                m_spawnedProp[propType].Add(newItem.GetComponent<Prop>());
            }
        }
    }
}