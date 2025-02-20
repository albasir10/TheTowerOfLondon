using GamePlay.Difficulties;
using System;

namespace Saves
{
    [Serializable]
    public class SaveResultData
    {
        public DifficultyType LastDifficultyType = DifficultyType.Easy;

        public int BestScoreEasy = 0;

        public int BestScoreNormal = 0;

        public int BestScoreHard = 0;

        public SaveResultsStruct[] SaveResults = new SaveResultsStruct[0];
    }
}
