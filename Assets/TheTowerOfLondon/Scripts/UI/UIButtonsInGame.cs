using Loaders;
using Services;
using UnityEngine;
using UnityEngine.UI;

namespace UIGame
{
    public class UIButtonsInGame : MonoBehaviour
    {
        [SerializeField] private Button _btnExit;

        [SerializeField] private Button _btnRestart;

        private void OnEnable()
        {
            _btnExit.onClick.AddListener(ExitToMenu);
            _btnRestart.onClick.AddListener(RestartLevel);
        }

        private void OnDisable()
        {
            _btnExit.onClick.RemoveListener(ExitToMenu);
            _btnRestart.onClick.RemoveListener(RestartLevel);
        }

        private void ExitToMenu()
        {
            GameService.Singleton.GetService<ISceneLoader>().LoadMenu();
        }

        private void RestartLevel()
        {
            GameService.Singleton.GetService<ISceneLoader>().LoadGame();
        }
    }
}
