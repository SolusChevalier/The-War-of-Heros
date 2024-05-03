using System;

public static class EventManager
{
    public static Action OnGamePaused;
    public static Action OnGameResumed;
    public static Action OnGameStarted;
    public static Action OnGameEnded;
    public static Action<int> PlayerTurn;
    public static Action<int> PlayerWin;
}