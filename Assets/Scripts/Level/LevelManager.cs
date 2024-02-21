using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
    public LevelContainer LevelContainer;
    public int CurrentLevelIndex { get; private set; }

    public GridContainer GetCurrentLevelData()
    {
        return LevelContainer.Levels[CurrentLevelIndex];
    }

    public void CompleteLevel()
    {
        CurrentLevelIndex++;
        CurrentLevelIndex = CurrentLevelIndex % LevelContainer.Levels.Count;
    }
}
