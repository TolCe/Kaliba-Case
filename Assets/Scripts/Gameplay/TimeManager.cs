using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void ChangeTimeScale(float multiplier)
    {
        Time.timeScale *= multiplier;
        SetDelta();
    }
    public void ResetTimeScale()
    {
        Time.timeScale = 1;
        SetDelta();
    }

    private void SetDelta()
    {
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }
}
