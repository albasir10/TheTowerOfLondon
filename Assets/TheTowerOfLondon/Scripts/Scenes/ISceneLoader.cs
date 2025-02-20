namespace Loaders
{
    public interface ISceneLoader
    {
        public void LoadAsync(string nameScene);

        public void LoadMenu();

        public void LoadGame();
    }
}
