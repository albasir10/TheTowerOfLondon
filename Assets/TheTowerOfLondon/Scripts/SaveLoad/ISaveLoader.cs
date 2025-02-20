using Saves;

namespace Loaders
{
    public interface ISaveLoader 
    {

        public void LoadAll();

        public void SaveAll();

        public void Save(SaveType saveType, object value);

        public object Load(LoadType loadType);
    }
}
