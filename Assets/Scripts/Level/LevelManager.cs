using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
    [Header("Level Data Container")]
    public LevelContainer levelContainer;

    private void Start()
    {
        SetLevelStuff();
    }

    private void SetLevelStuff()
    {
        LevelItemsManager.Instance.Initialize();
        GridManager.Instance.CreateGrid();
    }
}
