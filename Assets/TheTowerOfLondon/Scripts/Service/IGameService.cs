using System;

namespace Services
{
    public interface IGameService
    {
        public void Register<T>(object obj) where T : class;

        public void UnRegister<T>() where T : class;

        public T GetService<T>() where T : class;
    }
}
