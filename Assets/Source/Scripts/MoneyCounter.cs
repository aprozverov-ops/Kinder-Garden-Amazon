using System;
using System.Collections;
using System.Collections.Generic;
using EventBusSystem;
using Kuhpik;
using TMPro;
using UnityEngine;

public class MoneyCounter : GameSystem, IUpdateMoney
{
    [SerializeField] private List<TMP_Text> moneyText;
    [SerializeField] private float addMoneyDuration;

    private void OnEnable()
    {
        EventBus.Subscribe(this);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe(this);
    }

    public override void OnInit()
    {
        foreach (var money in moneyText)
        {
            money.text = player.Money.ToString();
        }
    }

    protected virtual IEnumerator UpdateMoneyCounter(float currentAmountMoney)
    {
        var currentMoney = Convert.ToInt32(moneyText[0].text);
        var reduce = currentMoney - currentAmountMoney;
        if (reduce < 0) reduce *= -1;
        reduce /= 100;
        if (reduce < 1) reduce = 1;
        while (currentMoney != currentAmountMoney)
        {
            if (currentMoney > currentAmountMoney)
            {
                currentMoney -= Convert.ToInt32(reduce);
                if (currentMoney < currentAmountMoney)
                {
                    currentMoney = Convert.ToInt32(currentAmountMoney);
                }
            }
            else
            {
                currentMoney += Convert.ToInt32(reduce);
                if (currentMoney > currentAmountMoney)
                {
                    currentMoney = Convert.ToInt32(currentAmountMoney);
                }
            }

            foreach (var money in moneyText)
            {
                money.text = currentMoney.ToString();
            }

            yield return new WaitForSeconds(addMoneyDuration);
        }
    }

    public void UpdateMoney()
    {
        StopAllCoroutines();
        StartCoroutine(UpdateMoneyCounter(player.Money));
    }
}