using GamePlay.Info;
using Services;
using TMPro;
using UnityEngine;

namespace UIGame
{
    public class UIInfoRemainingCount : MonoBehaviour
    {
        private TextMeshProUGUI _tmp;

        private IGameInfo _gameInfo;

        private void Awake()
        {
            _gameInfo = GameService.Singleton.GetService<IGameInfo>();

            _tmp = GetComponent<TextMeshProUGUI>();
        }

        private void Start()
        {

            UpdateText();
        }

        private void OnEnable()
        {
            _gameInfo.OnGrabbedToNewPost += UpdateText;
        }

        private void OnDisable()
        {
            _gameInfo.OnGrabbedToNewPost -= UpdateText;
        }

        private void UpdateText()
        {
            _tmp.SetText(_gameInfo.CountRemaining.ToString());      
        }
    }
}
