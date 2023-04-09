using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.XR;

public class GlobalDataManager
{
    public static int CurrentPlayerId = 1;
    public static GameState currentState= GameState.None;

    public enum GameState
    {
        None = 0,   
        cameraOpend=1,
        cameraPhotoTaked=2,
    }
         
    public static void ChangeState(GameState state)
    {
        currentState=state;
        EventCenter.Broadcast<GameState>(MyEventType.stateChanged, state);  
    }
}
