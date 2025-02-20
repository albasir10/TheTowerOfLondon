using GamePlay.Difficulties;
using System;
using UnityEngine;

namespace Settings
{
    [Serializable]
    public struct SettingsByTypeStruct
    {
        [SerializeField] private DifficultyType _difficulty;

        public DifficultyType Difficulty { get { return _difficulty; } }

        [SerializeField] private int _maxSwapPlaces;

        public int MaxSwapPlaces { get { return _maxSwapPlaces; } }

        [SerializeField] private int _additiveSwapPlaces;

        public int AdditiveSwapPlaces { get { return _additiveSwapPlaces; } }

        [SerializeField] private int _maxTorus;

        public int MaxTorus { get { return _maxTorus; } }
    }
}
