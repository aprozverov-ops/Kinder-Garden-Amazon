using System;
using System.Collections.Generic;
using DG.Tweening;
using Kuhpik;
using UnityEngine;
using Random = UnityEngine.Random;

public class MoneySpawnSystem : GameSystem
{
    [SerializeField] private Transform firstRoomMoneySpawnPos;
    [SerializeField] private Transform secondRoomMoneySpawnPos;
    [SerializeField] private Money moneyPrefab;
    [SerializeField] private CharacterConfigurations characterConfigurations;
    [SerializeField] private float radius;

    private IPool<Money> moneyPool;

    private void Awake()
    {
        var factory = new FactoryMonoObject<Money>(moneyPrefab.gameObject, transform);
        moneyPool = new Pool<Money>(factory, 3);
    }

    public void SpawnMoney(Transform spawnPosition, bool isSecond, int amount, int addMoney)
    {
        addMoney += Convert.ToInt32(addMoney * (characterConfigurations.AddMoneyPercentPerLevel *
                                                (player.UpgadeLevel[UpgradeType.Income] - 1)));
        if (player.IsTutorialFinish == false)
        {
            return;
        }

        var spawnedMoney = new List<Money>();
        var currentAdd = 0;
        for (int i = 0; i < amount; i++)
        {
            spawnedMoney.Add(moneyPool.Pull());
        }

        var add = addMoney / amount;
        foreach (var money in spawnedMoney)
        {
            money.SetMoney(add);
            money.Rigidbody.isKinematic = true;
            money.transform.position = spawnPosition.position + new Vector3(0, 3, 0);
            money.transform.rotation = Quaternion.Euler(new Vector3(Random.Range(-360f, 360f),
                Random.Range(-360f, 360f), Random.Range(-360f, 360f)));
            var pos = isSecond ? secondRoomMoneySpawnPos.position : firstRoomMoneySpawnPos.position;
            pos += new Vector3(Random.Range(-radius / 2f, radius / 2f), 0f,
                Random.Range(-radius / 2f, radius / 2f));
            money.transform.DOJump(pos, Random.Range(2f, 3.5f), 1, Random.Range(1f, 1.5f))
                .OnComplete(() => money.Rigidbody.isKinematic = false);
            currentAdd += add;
        }

        if (currentAdd != addMoney)
        {
            if (currentAdd > addMoney)
            {
                while (currentAdd > addMoney)
                {
                    currentAdd--;
                    spawnedMoney[Random.Range(0, spawnedMoney.Count)].SetMoney(add - 1);
                }
            }
            else
            {
                while (currentAdd < addMoney)
                {
                    currentAdd++;
                    spawnedMoney[Random.Range(0, spawnedMoney.Count)].SetMoney(add + 1);
                }
            }
        }
    }
}