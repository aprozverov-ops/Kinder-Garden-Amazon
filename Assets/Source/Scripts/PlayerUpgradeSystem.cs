using System;
using System.Collections;
using System.Collections.Generic;
using EventBusSystem;
using Kuhpik;
using UnityEngine;

public class PlayerUpgradeSystem : GameSystem, IRecalculateAllShopPanelSignal
{
    [SerializeField] private List<PlayerUpgradeConfiguration> playerUpgradeConfigurations;

    public override void OnInit()
    {
        Reinitialize();
    }

    private void OnEnable()
    {
        EventBus.Subscribe(this);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe(this);
    }

    private void Reinitialize()
    {
        var currentLvel = GetCurrentLevel();
        PlayerUpgradeConfiguration playerFound = null;
        foreach (var playerUpgradeConfiguration in playerUpgradeConfigurations)
        {
            if (currentLvel >= playerUpgradeConfiguration.MinLvl1 &&
                currentLvel <= playerUpgradeConfiguration.MaxLvl1)
            {
                playerFound = playerUpgradeConfiguration;
            }
            else
            {
                foreach (var lvlObject in playerUpgradeConfiguration.LvlObject)
                {
                    lvlObject.SetActive(false);
                }
            }
        }

        foreach (var lvlObject in playerFound.LvlObject)
        {
            lvlObject.SetActive(true);
        }
    }

    private int GetCurrentLevel()
    {
        var currentLvel = player.UpgadeLevel[UpgradeType.Stack] + player.UpgadeLevel[UpgradeType.Speed];
        return currentLvel;
    }

    public void Recalculate()
    {
        Debug.Log("123");
        Reinitialize();
    }
}

[Serializable]
public class PlayerUpgradeConfiguration
{
    [SerializeField] private List<GameObject> lvlObject;
    [SerializeField] private int MinLvl;
    [SerializeField] private int MaxLvl;

    public List<GameObject> LvlObject => lvlObject;

    public int MinLvl1 => MinLvl;

    public int MaxLvl1 => MaxLvl;
}