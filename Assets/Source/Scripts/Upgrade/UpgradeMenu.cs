using System;
using System.Collections;
using System.Collections.Generic;
using EventBusSystem;
using Kuhpik;
using UnityEngine;

public class UpgradeMenu : MonoBehaviour, IRecalculateAllShopPanelSignal
{
    [SerializeField] private PriceConfiguration priceConfiguration;
    [SerializeField] private UpgradeType upgradeType;
    [SerializeField] private List<PanelConfiguration> panels;

    private PlayerData playerData;
    private Transform player;
    private int price;
    private float currentPric;
    private PriceMakerConfiguration priceMakerConfiguration;

    private void OnEnable()
    {
        playerData ??= Bootstrap.Instance.PlayerData;
        Recalculate();
        EventBus.Subscribe(this);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe(this);
    }

    public void Recalculate()
    {
        var currentLevel = playerData.UpgadeLevel[upgradeType];
        priceMakerConfiguration = GetPriceMage();
        price = GeneratePrice(priceMakerConfiguration);
        SetStatus(playerData.Money >= price && currentLevel < priceMakerConfiguration.MAXLevel);
        foreach (var panel in panels)
        {
            var param = "";

            switch (LocalizationManager.Instance.GetCurrentLanguage())
            {
                case Languages.Russian:
                    param = currentLevel >= priceMakerConfiguration.MAXLevel ? "Макс" : price.ToString();
                    break;
                case Languages.English:
                    param = currentLevel >= priceMakerConfiguration.MAXLevel ? "Max" : price.ToString();
                    break;
                case Languages.Turkish:
                    param = currentLevel >= priceMakerConfiguration.MAXLevel ? "Maks." : price.ToString();
                    break;
            }

            panel.SetParam(currentLevel.ToString(), param);
        }
    }

    public int GeneratePrice(PriceMakerConfiguration priceMakerConfiguration)
    {
        if (Bootstrap.Instance.PlayerData.IsTutorialFinish == false &&
            playerData.UpgadeLevel[upgradeType] == 1 && upgradeType == UpgradeType.Income) return 10;
        var returnValue = priceMakerConfiguration.StartPrice;
        for (int i = 0; i < playerData.UpgadeLevel[upgradeType] - 1; i++)
        {
            returnValue += Convert.ToInt32(returnValue * priceMakerConfiguration.AddPercentPerLevel);
        }

        return returnValue;
    }

    public PriceMakerConfiguration GetPriceMage()
    {
        switch (upgradeType)
        {
            case UpgradeType.Speed:
                return priceConfiguration.SpeedLevelConfiguration;
                break;
            case UpgradeType.Stack:
                return priceConfiguration.StackLevelConfiguration;
                break;
            case UpgradeType.Income:
                return priceConfiguration.IncomeConfiguration1;
                break;
        }

        return null;
    }

    private void SetStatus(bool isActivate)
    {
        foreach (var panel in panels)
        {
            panel.SetActivate(isActivate);
        }
    }

    public void TryToBuy()
    {
        if (price > playerData.Money || playerData.UpgadeLevel[upgradeType] == priceMakerConfiguration.MAXLevel) return;
        playerData.Money -= price;
        playerData.UpgadeLevel[upgradeType]++;
        EventBus.RaiseEvent<IRecalculateAllShopPanelSignal>(t => t.Recalculate());
        Bootstrap.Instance.SaveGame();
        player ??= FindObjectOfType<Stack>().transform;
        Bootstrap.Instance.GetSystem<EffectSpawner>().SpawnEffect(EffectType.LevelUp, player);
        EventBus.RaiseEvent<IUpdateMoney>(t => t.UpdateMoney());
        VibrationSystem.Play();
    }
}