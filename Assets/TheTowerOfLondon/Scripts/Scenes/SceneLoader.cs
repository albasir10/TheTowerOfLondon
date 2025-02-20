using Bootstraps;
using Services;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Loaders
{
    public class SceneLoader : AbstractBootstrap, ISceneLoader
    {
        private bool _isLoading = false;

        [SerializeField] private string _nameMenuScene;

        [SerializeField] private string _nameGameScene;

        protected override void Init()
        {
           
        }

        public override void RegisterInService()
        {
            GameService.Singleton.Register<ISceneLoader>(this);
        }

        public override void AfterInitAndRegitster()
        {

        }

        [ContextMenu("Load Menu")]
        public void LoadMenu()
        {
            LoadAsync(_nameMenuScene);
        }

        [ContextMenu("Load Game")]
        public void LoadGame()
        {
            LoadAsync(_nameGameScene);
        }


        public void LoadAsync(string nameScene)
        {
            if (_isLoading)
            {
                return;
            }

            StartCoroutine(Loading(nameScene));
        }

        private IEnumerator Loading(string nameScene)
        {
            _isLoading = true;

            AsyncOperation loadingAsyncOperation = SceneManager.LoadSceneAsync(nameScene);

            loadingAsyncOperation.allowSceneActivation = false;

            while (loadingAsyncOperation.progress < 0.9f)
            {
                yield return null;
            }

            loadingAsyncOperation.allowSceneActivation = true;

            _isLoading = false;
        }


    }
}
