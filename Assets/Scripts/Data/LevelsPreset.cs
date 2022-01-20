using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "LevelsPreset", menuName = "Game Template/Levels Preset", order = 0)]
    public class LevelsPreset : ScriptableObject
    {
        public List<LevelItem> levels = new List<LevelItem>();

        [Serializable]
        public struct LevelItem
        {
            public string sceneName;
            public bool isRandom;
        }
    }
}