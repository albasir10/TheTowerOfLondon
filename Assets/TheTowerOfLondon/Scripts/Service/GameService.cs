using Bootstraps;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Services
{
    public class GameService : AbstractBootstrap, IGameService
    {
        private readonly Dictionary<Type, object> _dictInterfaceByType = new();

        private readonly Dictionary<Type, IResetStats> _dictInterfaceByResetStats = new();

        public static IGameService Singleton;
         
        protected override void Init()
        {
            Singleton = this;

            SceneManager.activeSceneChanged += ChangeScene;
        }

        public override void RegisterInService()
        {

        }

        public override void AfterInitAndRegitster()
        {

        }

        private void ChangeScene(Scene oldScene, Scene newScene)
        {
            ResetStats();
        }

        private void ResetStats()
        {
            foreach(IResetStats resetStats in _dictInterfaceByResetStats.Values)
            {
                resetStats.ResetStats();
            }
        }

        public T GetService<T>() where T : class
        {
            Type type = typeof(T);

            if (_dictInterfaceByType.ContainsKey(type))
            {
                return (T) _dictInterfaceByType[type];
            }
            else
            {
                return default;
            }
        }

        public void Register<T>(object obj) where T : class
        {
            Type type = typeof(T);

            if (!type.IsInterface || !obj.GetType().IsClass)
            {
                return;
            }

            if (!_dictInterfaceByType.ContainsKey(type))
            {
                _dictInterfaceByType.Add(type, obj);

                if (obj is MonoBehaviour objMono && objMono.TryGetComponent(out IResetStats resetStats))
                {
                    _dictInterfaceByResetStats.Add(type, resetStats);
                }

            }
        }

        public void UnRegister<T>() where T : class
        {
            Type type = typeof(T);

            if (!type.IsInterface)
            {
                return;
            }

            if (_dictInterfaceByType.ContainsKey(type))
            {
                _dictInterfaceByType.Remove(type);

                _dictInterfaceByResetStats.Remove(type);
            }
        }


    }
}
