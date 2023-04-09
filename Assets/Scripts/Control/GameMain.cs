using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMain : MonoBehaviour
{
    private void Awake()
    {
        InitGame();

        InitGameState();
    }

    private void InitGame()
    {
        PlayerManager.Instance.CreatePlayer(GlobalDataManager.CurrentPlayerId);
    }

    private void InitGameState()
    {
        GlobalDataManager.ChangeState(GlobalDataManager.GameState.None);
    }
}
