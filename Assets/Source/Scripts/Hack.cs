using System;
using System.Collections;
using System.Collections.Generic;
using EventBusSystem;
using Kuhpik;
using UnityEngine;
using UnityEngine.UI;

public class Hack : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            Bootstrap.Instance.PlayerData.Money += 1000;
            EventBusSystem.EventBus.RaiseEvent<IUpdateMoney>(t => t.UpdateMoney());
        });
    }
}
