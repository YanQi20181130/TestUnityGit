using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private Dictionary<string , Renderer> playerPartRenderDic;


    public void InitPlayer()
    {
        playerPartRenderDic=new Dictionary<string , Renderer>();

        foreach (var item in transform.GetComponentsInChildren<Renderer>(true))
        {
            playerPartRenderDic.Add(item.name , item.GetComponent<Renderer>());
        }
        EventCenter.AddListener<Texture2D>(MyEventType.setTextureToPlayer , OnSetTexture);
    }

    private void OnSetTexture(Texture2D tex)
    {
        Dictionary<string , Color> _mapDic=new Dictionary<string , Color>();
        SetTextureToPlayer(playerPartRenderDic , _mapDic);
    }

    public void SetTextureToPlayer(Dictionary<string , Renderer> _playerPartRenderDic , Dictionary<string , Color> _mapDic)
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
    // Update is called once per frame 
    void Update()
    {
        SetPlayerPos();

    }

    private void SetPlayerPos()
    {
#if UNITY_EDITOR
        //如果鼠标左键按下
        if (Input.GetMouseButton(0))
        {
            float speed = 2.5f;//旋转跟随速度
            float OffsetX = Input.GetAxis("Mouse X");//获取鼠标x轴的偏移量
            float OffsetY = Input.GetAxis("Mouse Y");//获取鼠标y轴的偏移量
            transform.Rotate(new Vector3(OffsetY , -OffsetX , 0)*speed , Space.World);//旋转物体
        }
#else
        //没有触摸  
        if (Input.touchCount<=0)
        {
            return;
        }
        else if (1==Input.touchCount)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 deltaPos = touch.deltaPosition;//位置增量

            if (Mathf.Abs(deltaPos.x)>=3||Mathf.Abs(deltaPos.y)>=3)
            {

                transform.Rotate(Vector3.down*deltaPos.x , Space.World);//绕y轴旋转
                transform.Rotate(Vector3.right*deltaPos.y , Space.World);//绕x轴
            }
        }
#endif
    }
}
