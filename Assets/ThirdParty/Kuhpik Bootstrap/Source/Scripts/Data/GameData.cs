using System;
using UnityEngine;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;

namespace Kuhpik
{
    /// <summary>
    /// Used to store game data. Change it the way you want.
    /// </summary>
    [Serializable]
    public class GameData
    {
        public int amountChildFirst;
        public int amountChildFirstFake;
        public int amountChildSecond;
        public int amountChildSecondFake;
        public bool isSecondRoom;
        public CharacterAnimationController CharacterAnimationController;
    }
}