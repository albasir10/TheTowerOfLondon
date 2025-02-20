using Loaders;
using Services;
using UnityEngine;
using UnityEngine.UI;

namespace UIMenu
{
    public class UIMainMenuGeneral : MonoBehaviour
    {
        [SerializeField] private Button _btnStartGame;
        [SerializeField] private Button _btnDif;
        [SerializeField] private Button _btnScores;
        [SerializeField] private Button _btnExit;

        [SerializeField] private GameObject _panelDif;
        [SerializeField] private GameObject _panelScores;

        private void OnEnable()
        {
            _btnStartGame.onClick.AddListener(StartGame);
            _btnDif.onClick.AddListener(ChangePanelToDifficutly);
            _btnScores.onClick.AddListener(ChangePanelToScores);
            _btnExit.onClick.AddListener(ExitGame);
        }

        private void OnDisable()
        {
            _btnStartGame.onClick.RemoveListener(StartGame);
            _btnDif.onClick.RemoveListener(ChangePanelToDifficutly);
            _btnScores.onClick.RemoveListener(ChangePanelToScores);
            _btnExit.onClick.RemoveListener(ExitGame);
        }


        private void StartGame()
        {
            GameService.Singleton.GetService<ISceneLoader>().LoadGame();
        }

        private void ChangePanelToDifficutly()
        {
            _panelDif.SetActive(true);
            gameObject.SetActive(false);
        }

        private void ChangePanelToScores()
        {
            _panelScores.SetActive(true);
            gameObject.SetActive(false);
        }

        private void ExitGame()
        {
            Application.Quit();
        }

    }
}
