using System;

public static class BusSystem
{
    public static class Level
    {
        public static Action OnLevelLoad;
        public static void CallLevelLoad() { OnLevelLoad?.Invoke(); }

        public static Action OnLevelSuccess;
        public static void CallLevelSuccess() { OnLevelSuccess?.Invoke(); }

        public static Action OnLevelUnload;
        public static void CallLevelUnload() { OnLevelUnload?.Invoke(); }
    }
}
