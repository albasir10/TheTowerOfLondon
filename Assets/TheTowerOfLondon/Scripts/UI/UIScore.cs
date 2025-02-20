using GamePlay.Info;
using Loaders;
using Services;
using TMPro;
using UnityEngine;

namespace UIGame
{
    public class UIScore : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _tmpInfo;

        [SerializeField] private TextMeshProUGUI _tmpScore;

        [SerializeField] private TextMeshProUGUI _tmpNewMaxScore;

        private IGameInfo _gameInfo;

        private void Awake()
        {
            _gameInfo = GameService.Singleton.GetService<IGameInfo>();

            _tmpScore.enabled = false;

            _tmpNewMaxScore.enabled = false;

            _tmpInfo.enabled = false;
        }

        private void OnEnable()
        {
            _gameInfo.OnGameEnd += UpdateText;
            _gameInfo.OnNewRecord += NewRecord;
        }

        private void OnDisable()
        {
            _gameInfo.OnGameEnd -= UpdateText;
            _gameInfo.OnNewRecord -= NewRecord;
        }

        private void UpdateText(bool isWin, int score)
        {
            _tmpInfo.enabled = true;

            _tmpScore.enabled = true;

            _tmpScore.SetText(score.ToString());
        }

        private void NewRecord()
        {
            _tmpNewMaxScore.enabled = true;
        }
    }
}
