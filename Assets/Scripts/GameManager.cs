public class GameManager : Singleton<GameManager>
{
    private void Start()
    {
        LoadLevel(LevelManager.Instance.GetCurrentLevelData());
    }

    public void LoadLevel(GridContainer gridData)
    {
        BusSystem.Level.CallLevelLoad();
        LevelItemsManager.Instance.Initialize();
        GridManager.Instance.CreateGrid(gridData);
    }

    public void ResetLevel()
    {
        BusSystem.Level.CallLevelUnload();
    }

    public void LoadNextLevel()
    {
        ResetLevel();
        LevelManager.Instance.CompleteLevel();
        LoadLevel(LevelManager.Instance.GetCurrentLevelData());
    }
}
