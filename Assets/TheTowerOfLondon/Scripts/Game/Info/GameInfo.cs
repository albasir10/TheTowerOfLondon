using Bootstraps;
using GamePlay.Difficulties;
using Loaders;
using Services;
using Settings;
using System;
using UnityEngine;

namespace GamePlay.Info
{
    public class GameInfo : AbstractBootstrap, IGameInfo, IResetStats
    {

        [SerializeField] private SettingsByTypeStruct[] _settingsByType;

        [SerializeField] private GameInfoStruct _gameInfoStruct;

        public GameInfoStruct GameInfoStruct { get { return _gameInfoStruct; } }

        public int CountRemaining { get { return _gameInfoStruct.Settings.MaxSwapPlaces + _gameInfoStruct.Settings.AdditiveSwapPlaces - _currentGrabbedToNewPost; } }

        private int _currentSuccessPost = 0;

        private int _maxSuccessPost = 3;

        private int _currentGrabbedToNewPost = 0;

        public event Action<bool, int> OnGameEnd;

        public event Action OnGrabbedToNewPost;

        public event Action OnNewRecord;

        protected override void Init()
        {
           
        }

        public override void RegisterInService()
        {
            GameService.Singleton.Register<IGameInfo>(this);
        }

        public void SetDifficulty(DifficultyType difficulty)
        {
            _gameInfoStruct.Difficulty = difficulty;

            foreach(SettingsByTypeStruct settingsByType in _settingsByType)
            {
                if (difficulty == settingsByType.Difficulty)
                {
                    _gameInfoStruct.Settings = settingsByType;
                }
            }
        }

        public override void AfterInitAndRegitster()
        {
            SetDifficulty((DifficultyType) GameService.Singleton.GetService<ISaveLoader>().Load(LoadType.Difficulty));
        }

        public void AddSuccessPost()
        {
            _currentSuccessPost++;
        }


        public void RemoveSuccessPost()
        {
            _currentSuccessPost--;
        }

        public void ResetStats()
        {
            _currentSuccessPost = 0;

            _currentGrabbedToNewPost = 0;
        }

        public void GrabbedToNewPost()
        {
            _currentGrabbedToNewPost++;

            OnGrabbedToNewPost?.Invoke();

            bool isWin = _currentSuccessPost == _maxSuccessPost;

            if (CountRemaining == 0 || isWin)
            {
                EndGame(isWin);
            }
        }

        private void EndGame(bool isWin)
        {
            int score = isWin ? 100 * (CountRemaining + 1) : 0;

            OnGameEnd?.Invoke(isWin, score);

            if (isWin)
            {
                int recordScore = 0;

                switch (_gameInfoStruct.Difficulty)
                {
                    case DifficultyType.Easy:

                        recordScore = (int)GameService.Singleton.GetService<ISaveLoader>().Load(LoadType.RecordEasy);

                        break;

                    case DifficultyType.Normal:

                        recordScore = (int)GameService.Singleton.GetService<ISaveLoader>().Load(LoadType.RecordNormal);

                        break;

                    case DifficultyType.Hard:

                        recordScore = (int)GameService.Singleton.GetService<ISaveLoader>().Load(LoadType.RecordHard);

                        break;
                }

                if (recordScore < score)
                {
                    OnNewRecord?.Invoke();
                }

                GameService.Singleton.GetService<ISaveLoader>().Save(Saves.SaveType.Result, score);
            } 
        }
    }
}
