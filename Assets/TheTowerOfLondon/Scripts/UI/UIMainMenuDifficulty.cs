using GamePlay.Difficulties;
using GamePlay.Info;
using Services;
using UnityEngine;
using UnityEngine.UI;

namespace UIMenu
{
    public class UIMainMenuDifficulty : MonoBehaviour
    {
        [SerializeField] private Button _btnBack;

        [SerializeField] private Button _btnEasy;

        [SerializeField] private Button _btnNormal;

        [SerializeField] private Button _btnHard;

        [SerializeField] private GameObject _panelGeneral;

        private Button _lastBtn;

        private void Awake()
        {
            DisableActiveDifficultyBtn();
        }

        private void OnEnable()
        {
            _btnBack.onClick.AddListener(BackToGeneral);
            _btnEasy.onClick.AddListener(SetEasy);
            _btnNormal.onClick.AddListener(SetNormal);
            _btnHard.onClick.AddListener(SetHard);
        }

        private void OnDisable()
        {
            _btnBack.onClick.RemoveListener(BackToGeneral);
            _btnEasy.onClick.RemoveListener(SetEasy);
            _btnNormal.onClick.RemoveListener(SetNormal);
            _btnHard.onClick.RemoveListener(SetHard);
        }

        private void BackToGeneral()
        {
            _panelGeneral.SetActive(true);
            gameObject.SetActive(false);
        }

        private void SetEasy()
        {
            GameService.Singleton.GetService<IGameInfo>().SetDifficulty(DifficultyType.Easy);

            DisableActiveDifficultyBtn();
        }

        private void SetNormal()
        {
            GameService.Singleton.GetService<IGameInfo>().SetDifficulty(DifficultyType.Normal);

            DisableActiveDifficultyBtn();
        }

        private void SetHard()
        {
            GameService.Singleton.GetService<IGameInfo>().SetDifficulty(DifficultyType.Hard);

            DisableActiveDifficultyBtn();
        }

        private void DisableActiveDifficultyBtn()
        {
            if (_lastBtn != null)
            {
                _lastBtn.interactable = true;
            }

            switch (GameService.Singleton.GetService<IGameInfo>().GameInfoStruct.Difficulty)
            {
                case DifficultyType.Easy:

                    _lastBtn = _btnEasy;

                    break;
                case DifficultyType.Normal:

                    _lastBtn = _btnNormal;

                    break;
                case DifficultyType.Hard:

                    _lastBtn = _btnHard;

                    break;
            }

            _lastBtn.interactable = false;
        }
    }
}
