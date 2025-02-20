using Loaders;
using Services;
using UnityEngine;

namespace Bootstraps
{
    public class Bootstrap : MonoBehaviour
    {
        private void Awake()
        {
            Object[] objects = Resources.LoadAll("BootsTrap", typeof(GameObject));

            AbstractBootstrap[] abstractBootstraps = new AbstractBootstrap[objects.Length];

            GameObject tempObj;

            for (int i = 0; i < objects.Length; i++)
            {
                tempObj = Instantiate((GameObject)objects[i]);

                tempObj.name = objects[i].name;

                if (tempObj.TryGetComponent(out AbstractBootstrap abstractBootstrap))
                {
                    abstractBootstraps[i] = abstractBootstrap;

                    abstractBootstrap.MainInit();
                   
                }
            }

            foreach(AbstractBootstrap abstractBootstrap in abstractBootstraps)
            {
                abstractBootstrap.RegisterInService();
            }

            foreach (AbstractBootstrap abstractBootstrap in abstractBootstraps)
            {
                abstractBootstrap.AfterInitAndRegitster();
            }

            GameService.Singleton.GetService<ISceneLoader>().LoadMenu();  
        }
    }
}
