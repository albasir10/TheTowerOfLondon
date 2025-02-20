using GamePlay.Difficulties;
using Settings;
using System;
using UnityEngine;

namespace GamePlay.Info
{
    [Serializable]
    public struct GameInfoStruct
    {
        public DifficultyType Difficulty;

        public SettingsByTypeStruct Settings;
    }
}
