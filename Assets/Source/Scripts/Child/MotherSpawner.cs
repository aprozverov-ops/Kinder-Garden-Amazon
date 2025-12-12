using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Kuhpik;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class MotherSpawner : GameSystem
{
    [SerializeField] private MotherSpawnConfiguration firstChildStage;
    [SerializeField] private MotherSpawnConfiguration secondChildStage;

    public MotherSpawnConfiguration FirstChildStage => firstChildStage;

    public MotherSpawnConfiguration SecondChildStage => secondChildStage;

    public override void OnInit()
    {
        firstChildStage.OnInit(transform);
        secondChildStage.OnInit(transform);
        StartCoroutine(SpawnChildFirst());
        StartCoroutine(SpawnChildSecond());
    }

    private IEnumerator SpawnChildFirst()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0, 4f));
            if (firstChildStage.IsCanBeSpawn || player.IsTutorialFinish == false) continue;
            firstChildStage.SpawnNewMother();
        }
    }

    private IEnumerator SpawnChildSecond()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0, 4f));
            if (secondChildStage.IsCanBeSpawn || player.IsTutorialFinish == false ||
                player.boughtPlaces.Contains(9) == false) continue;
            secondChildStage.SpawnNewMother();
        }
    }
}

[Serializable]
public class MotherSpawnConfiguration
{
    [SerializeField] private bool isSecond;
    [SerializeField] private List<GameObject> bed;
    [SerializeField] private List<Mother> mother;
    [SerializeField] private List<Transform> spawnPosition;
    [SerializeField] private List<Transform> endPoints;

    private readonly List<IPool<Mother>> m_motherPools = new List<IPool<Mother>>();

    public bool IsReadyToUnlockChild => AmountSpawnedChild() < AmountFreePlace();

    public bool IsCanBeSpawn => (isSecond
        ? Bootstrap.Instance.GameData.amountChildSecond
        : Bootstrap.Instance.GameData.amountChildFirst) >= IsAtivate();

    public void OnInit(Transform transform)
    {
        foreach (var motherPrefab in mother)
        {
            var factory = new FactoryMonoObject<Mother>(motherPrefab.gameObject, transform);
            var pool = new Pool<Mother>(factory, 1);
            m_motherPools.Add(pool);
        }
    }

    private int IsAtivate()
    {
        var amountActivateBed = bed.Count(t => t.gameObject.activeInHierarchy);
        if (amountActivateBed >= 4) amountActivateBed = 4;
        else amountActivateBed += 2;
        return amountActivateBed;
    }

    private int AmountSpawnedChild()
    {
        return isSecond
            ? Bootstrap.Instance.GameData.amountChildSecondFake
            : Bootstrap.Instance.GameData.amountChildFirstFake;
    }

    private int AmountFreePlace()
    {
        var amountActivateBed = bed.Count(t => t.gameObject.activeInHierarchy);
        return amountActivateBed+1;
    }

    [Button("SpawnMother")]
    public void SpawnNewMother()
    {
        if (Bootstrap.Instance.PlayerData.IsTutorialFinish == false) return;
        var newMother = m_motherPools[Random.Range(0, m_motherPools.Count)].Pull();
        newMother.MotherSpawner = this;
        newMother.GetComponent<NavMeshAgent>().enabled = false;
        var spawnPosition = this.spawnPosition[Random.Range(0, this.spawnPosition.Count)];
        newMother.transform.position = spawnPosition.position;
        newMother.MotherInitialize(endPoints[Random.Range(0, endPoints.Count)], isSecond);
    }

    public void SpawnMotherToTakeChild()
    {
        if (Bootstrap.Instance.PlayerData.IsTutorialFinish == false) return;
        var newMother = m_motherPools[Random.Range(0, m_motherPools.Count)].Pull();
        newMother.MotherSpawner = this;
        newMother.GetComponent<NavMeshAgent>().enabled = false;
        var spawnPosition = this.spawnPosition[Random.Range(0, this.spawnPosition.Count)];
        newMother.transform.position = spawnPosition.position;
        newMother.MotherTakeChild(endPoints[Random.Range(0, endPoints.Count)], isSecond);
    }
}