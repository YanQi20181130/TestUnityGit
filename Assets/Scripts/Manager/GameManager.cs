using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeChatWASM;
public class GameManager : Singleton<GameManager>
{
    public void LoadPlayer(int playerID)
    {

    }

    /// <summary>
    /// ������ͷ
    /// </summary>
    public void OpenWebCamDevice()
    {
        Debug.Log("�����");
        ChooseImageOption chooseMedia = new ChooseImageOption
        {
            complete=OnchooseMediaGeneral_complete_CallbackResult ,
            fail=OnchooseMediaGeneral_fail_CallbackResult ,
            sizeType=new string[1] { "compressed" } ,
            count=1 ,
            sourceType=new string[2] { "camera" , "album" } ,
            success=(result) =>
            {
                // ��ȡͼƬ��ַ
                var tempPath = result.tempFilePaths[0];
                // ��ͼƬ��ַȡͼƬ
                var bytes = WX.GetFileSystemManager().ReadFileSync(tempPath);
                Texture2D texture = new Texture2D(512 , 512);
                texture.LoadImage(bytes);
                //player.GetComponent<PlayerPos>().SetTextureToPlayer(player , texture);
            } ,
        };

        WX.ChooseImage(chooseMedia);
    }
    public void OnchooseMediaGeneral_complete_CallbackResult(GeneralCallbackResult re)
    {
        Debug.Log($"general complete  :    "+re.errMsg);
    }
    public void OnchooseMediaGeneral_fail_CallbackResult(GeneralCallbackResult re)
    {
        Debug.Log($"General failed  :    "+re.errMsg);
    }
}
