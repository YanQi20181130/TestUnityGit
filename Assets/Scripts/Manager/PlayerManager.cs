using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{

    public Dictionary<int,GameObject> playerDic = new Dictionary<int,GameObject>();
    public void CreatePlayer(int id)
    {
        if (!playerDic.ContainsKey(id))
        {
            var newPlayer = Instantiate(Resources.Load("Player/Player_"+id) as GameObject);
            newPlayer.AddComponent<PlayerControl>();
            newPlayer.GetComponent<PlayerControl>().InitPlayer();
            AddPlayer(id , newPlayer);
        }

        UpdatePlayer(id);
    }

    public void DestroyPlayer(int id) {
    if(playerDic.ContainsKey(id)) {

            DestroyImmediate(playerDic[id]);
            playerDic.Remove(id);

        }
    }

    public void UpdatePlayer(int id) {
        playerDic[id].transform.position=new Vector3(0 , 0 , 0);
        playerDic[id].SetActive(true );
    }


    public void AddPlayer(int id,GameObject _player) {
        if (playerDic.ContainsKey(id)==false) {
            playerDic.Add(id , _player);
        }
    } 
    
    public void RemovePlayer(int id) {
        if (playerDic.ContainsKey(id))
        {
            playerDic.Remove(id );

        }
    }


}
