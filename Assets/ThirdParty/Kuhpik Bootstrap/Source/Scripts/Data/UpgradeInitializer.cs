using System;
using System.Collections;
using System.Collections.Generic;
using Kuhpik;
using UnityEngine;

public class UpgradeInitializer : GameSystem
{
    public override void OnInit()
    {
        if (player.UpgadeLevel == null)
        {
            player.UpgadeLevel = new Dictionary<UpgradeType, int>();
            foreach (UpgradeType type in Enum.GetValues(typeof(UpgradeType)))
            {
                player.UpgadeLevel.Add(type, 1);
            }
        }

        StartCoroutine(InitializeAnalitycs());
    }

    private IEnumerator InitializeAnalitycs()
    {
        yield return new WaitForSeconds(4f);
        if (player.IsFristLaunch == false)
        {
            player.IsFristLaunch = true;
        }

        if (player.IsFristTutorial == false)
        {
            player.IsFristTutorial = true;
        }

        Bootstrap.Instance.SaveGame();
    }

    public void Coroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }
}