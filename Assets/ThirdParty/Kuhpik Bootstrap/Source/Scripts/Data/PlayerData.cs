using System;
using UnityEngine;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;

namespace Kuhpik
{
    /// <summary>
    /// Used to store player's data. Change it the way you want.
    /// </summary>
    [Serializable]
    public class PlayerData
    {
        public bool IsFristLaunch;
        public bool IsFristTutorial;
        
        
        public List<int> savedTutorSteps = new List<int>();
        public bool IsTutorialFinish;
        public bool IsVibration = true;
        public int Money;
        public Dictionary<UpgradeType, int> UpgadeLevel;
        public List<int> boughtPlaces = new List<int>();

        public void TryToSaveTutorSteps(int id, string path)
        {
            if (savedTutorSteps.Contains(id))
            {
                return;
            }

            savedTutorSteps.Add(id);

            Bootstrap.Instance.SaveGame();
        }
    }
}