using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    public void SetTextureToPlayer(Dictionary<string , Renderer> _playerPartRenderDic, Dictionary<string , Color>  _mapDic)
    {
        foreach (var part in _playerPartRenderDic)
        {
            foreach (var map in _mapDic)
            {
                if (map.Key.Equals(part.Key))
                {
                    part.Value.material.color=map.Value;
                    break;
                }
            }
        }
    }
}
