using GamePlay.Info;
using Services;
using TMPro;
using UnityEngine;

namespace UIGame
{
    public class UILoseOrWin : MonoBehaviour
    {
        private TextMeshProUGUI _tmp;

        private IGameInfo _gameInfo;

        private string _winText = "WIN";

        private string _loseText = "LOSE";

        private void Awake()
        {
            _gameInfo = GameService.Singleton.GetService<IGameInfo>();

            _tmp = GetComponent<TextMeshProUGUI>();

            _tmp.SetText("");
        }

        private void OnEnable()
        {
            _gameInfo.OnGameEnd += UpdateText;
        }

        private void OnDisable()
        {
            _gameInfo.OnGameEnd -= UpdateText;
        }

        private void UpdateText(bool isWin, int score)
        {
            _tmp.SetText(isWin ? _winText : _loseText);
        }
    }
}
