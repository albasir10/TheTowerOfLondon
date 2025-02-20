using Bootstraps;
using GamePlay.Difficulties;
using GamePlay.Info;
using Saves;
using Services;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Loaders
{
    public class SaveLoader : AbstractBootstrap, ISaveLoader
    {
        [SerializeField] private int _maxSlotsResult = 10;

        private string _path;

        private SaveResultData _saveData;

        protected override void Init()
        {
            _saveData = new();

            _path = Application.dataPath + "/SaveData.json";

            LoadAll();
        }

        public override void RegisterInService()
        {
            GameService.Singleton.Register<ISaveLoader>(this);
        }

        public override void AfterInitAndRegitster()
        {
            
        }


        #region Load

        public void LoadAll()
        {
            if (File.Exists(_path))
            {
                string json = File.ReadAllText(_path);

                _saveData = JsonUtility.FromJson<SaveResultData>(json);

                Debug.Log("Сохранения загружены");
            }
            else
            {
                Debug.Log("Файл сохранения не найден, создаем новый...");

                SaveAll();
            }
        }

        public object Load(LoadType loadType)
        {
            switch(loadType)
            {
                case LoadType.Difficulty:

                    return _saveData.LastDifficultyType;

                case LoadType.RecordEasy:

                    return _saveData.BestScoreEasy;

                case LoadType.RecordNormal:

                    return _saveData.BestScoreNormal;

                case LoadType.RecordHard:

                    return _saveData.BestScoreHard;

                case LoadType.Results:

                    return _saveData.SaveResults;

                default:

                    return default;
            }
        }

        #endregion Load


        #region Save

        public void Save(SaveType saveType, object value)
        {
            switch(saveType)
            {
                case SaveType.Result:

                    if (value is int valueInt)
                    {
                        UpdateMaxScore(valueInt);

                        AddNewScoreToResults(valueInt);
                    }

                    break;
                case SaveType.Difficulty:

                    if (value is DifficultyType difficulty)
                    {
                        _saveData.LastDifficultyType = difficulty;
                    }

                    break;
            }

            SaveAll();
        }

        private void UpdateMaxScore(int value)
        {
            switch(GameService.Singleton.GetService<IGameInfo>().GameInfoStruct.Difficulty)
            {
                case DifficultyType.Easy:

                    if (_saveData.BestScoreEasy < value)
                    {
                        _saveData.BestScoreEasy = value;
                    }

                    break;
                case DifficultyType.Normal:

                    if (_saveData.BestScoreNormal < value)
                    {
                        _saveData.BestScoreNormal = value;
                    }

                    break;
                case DifficultyType.Hard:

                    if (_saveData.BestScoreHard < value)
                    {
                        _saveData.BestScoreHard = value;
                    }

                    break;
            }
        }

        private void AddNewScoreToResults(int value)
        {
            SaveResultsStruct newResult = new()
            {
                Difficulty = GameService.Singleton.GetService<IGameInfo>().GameInfoStruct.Difficulty,
                result = value
            };

            int len = _saveData.SaveResults.Length + 1;


            if (len > _maxSlotsResult)
            {
                for (int i = 0; i < _saveData.SaveResults.Length - 1; i++)
                {
                    _saveData.SaveResults[i] = _saveData.SaveResults[i + 1];
                }

                _saveData.SaveResults[^1] = newResult;
            }
            else
            {
                SaveResultsStruct[] saveNewResults = new SaveResultsStruct[len];

                for (int i = 0; i < _saveData.SaveResults.Length; i++)
                {
                    saveNewResults[i] = _saveData.SaveResults[i];
                }

                saveNewResults[^1] = newResult;

                _saveData.SaveResults = saveNewResults;
            }


        }

        public void SaveAll()
        {
            string json = JsonUtility.ToJson(_saveData, true);

            File.WriteAllText(_path, json);
        }


        #endregion Save


        private void OnDestroy()
        {
            _saveData.LastDifficultyType = GameService.Singleton.GetService<IGameInfo>().GameInfoStruct.Difficulty;

            SaveAll();
        }
    }
}
