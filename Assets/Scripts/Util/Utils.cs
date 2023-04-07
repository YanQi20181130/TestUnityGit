using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeChatWASM;

public class Utils
{
    #region Open Media

    public static System.Action<Texture2D> OnCompleteGetPhoto;
    /// <summary>
    /// 打开摄像头
    /// </summary>
    public static void OpenWebCamDevice(System.Action<Texture2D> _onComplete)
    {
        Debug.Log("Openning camera !");

        if (_onComplete==null) return;

        ChooseImageOption chooseMedia = new ChooseImageOption
        {
            complete=OnchooseMediaGeneral_complete_CallbackResult ,
            fail=OnchooseMediaGeneral_fail_CallbackResult ,
            sizeType=new string[1] { "compressed" } ,
            count=1 ,
            sourceType=new string[2] { "camera" , "album" } ,
            success=OnChooseImgSucceResult
        };

        OnCompleteGetPhoto=_onComplete;

        WX.ChooseImage(chooseMedia);
    }
    public static void OnchooseMediaGeneral_complete_CallbackResult(GeneralCallbackResult re)
    {
        Debug.Log("Opened camera");
        Debug.Log($"general complete  :    "+re.errMsg);
    }
    public static void OnchooseMediaGeneral_fail_CallbackResult(GeneralCallbackResult re)
    {
        Debug.Log("Failed to open camera");
        Debug.Log($"General failed  :    "+re.errMsg);
    }

    public static void OnChooseImgSucceResult(ChooseImageSuccessCallbackResult result)
    {
        // 获取图片地址
        var tempPath = result.tempFilePaths[0];
        // 从图片地址取图片
        var bytes = WX.GetFileSystemManager().ReadFileSync(tempPath);
        Texture2D texture = new Texture2D(512 , 512);
        texture.LoadImage(bytes);
        if (OnCompleteGetPhoto!=null)
        {
            OnCompleteGetPhoto.Invoke(texture);
        }
        else
        {
            Debug.LogError("未找到角色");
        }
        //player.GetComponent<PlayerPos>().SetTextureToPlayer(player , texture);
    }
    #endregion

    public static void GetTextureColor(Texture2D tex)
    {

    }
}
