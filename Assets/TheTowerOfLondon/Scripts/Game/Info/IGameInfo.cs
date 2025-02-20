using GamePlay.Difficulties;
using System;

namespace GamePlay.Info
{
    public interface IGameInfo
    {
        public GameInfoStruct GameInfoStruct { get; }

        public int CountRemaining { get; }

        public event Action<bool, int> OnGameEnd;

        public event Action OnGrabbedToNewPost;

        public event Action OnNewRecord;

        public void SetDifficulty(DifficultyType difficulty);

        public void AddSuccessPost();

        public void RemoveSuccessPost();

        public void GrabbedToNewPost();
    }
}