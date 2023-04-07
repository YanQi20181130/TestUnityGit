
using System;
using UnityEngine;
public class PlayerPos : MonoBehaviour {

   public  void SetTextureToPlayer(MeshRenderer playerRender , Texture2D _texture2D)
    {
        if (playerRender==null)
        {
            return;
        }

        playerRender.material.mainTexture=_texture2D;

        Resources.UnloadUnusedAssets();
        GC.Collect();
    }

    // Update is called once per frame 
    void Update() {

        //没有触摸  
        if ( Input.touchCount <= 0 ) {
            return;
        }
        else if ( 1 == Input.touchCount ) {
            Touch touch = Input.GetTouch(0);
            Vector2 deltaPos = touch.deltaPosition;//位置增量

            if ( Mathf.Abs(deltaPos.x) >= 3 || Mathf.Abs(deltaPos.y) >= 3 ) {

                transform.Rotate(Vector3.down * deltaPos.x , Space.World);//绕y轴旋转
                transform.Rotate(Vector3.right * deltaPos.y , Space.World);//绕x轴
            }
        }
    }

}
