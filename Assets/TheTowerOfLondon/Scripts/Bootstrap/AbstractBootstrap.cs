using UnityEngine;

namespace Bootstraps
{
    public abstract class AbstractBootstrap : MonoBehaviour
    {
        private bool _isInit = false;

        public void MainInit()
        {
            if (_isInit)
            {
                return;
            }

            DontDestroyOnLoad(gameObject);

            Init();

            _isInit = true;
        }

        /// <summary>
        /// 1 этап Инициализация
        /// </summary>
        protected abstract void Init();

        /// <summary>
        /// 2 этап Регистрация
        /// </summary>
        public abstract void RegisterInService();

        /// <summary>
        /// 3 этап После Инициализации и Регистрации всех дочерних классов
        /// </summary>
        public abstract void AfterInitAndRegitster();
    }
}
