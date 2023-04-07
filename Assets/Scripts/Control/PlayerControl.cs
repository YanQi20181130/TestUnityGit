using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private Dictionary<string , Renderer> playerPartRenderDic;
    private void Awake()
    {
        InitPlayer();
    }

    private void InitPlayer()
    {
        playerPartRenderDic=new Dictionary<string , Renderer>();

        foreach (var item in transform.GetComponentsInChildren<Renderer>(true))
        {
            playerPartRenderDic.Add(item.name , item.GetComponent<Renderer>());
        }
    }


}
