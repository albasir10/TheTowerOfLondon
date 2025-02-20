using GamePlay.Difficulties;
using System;

namespace Saves
{
    [Serializable]
    public struct SaveResultsStruct 
    {
        public DifficultyType Difficulty;

        public int result;
    }
}
