using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMain : MonoBehaviour
{
    private void Awake()
    {
        InitGame();
    }

    private void InitGame()
    {
        PlayerManager.Instance.CreatePlayer(GlobalDataManager.CurrentPlayerId);
    }


}
