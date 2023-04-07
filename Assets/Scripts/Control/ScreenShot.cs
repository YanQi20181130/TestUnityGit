using System;
using System.IO;
using UnityEngine;

public class ScreenShot : MonoBehaviour {
    readonly string savePath = Application.streamingAssetsPath + "/my"; // 图片保存路径

    private MeshRenderer playerRender;

    public void SaveScreenShot(MeshRenderer _playerRender ,Texture _texture) {
        Texture2D local_jietuTexture = TextureToTexture2D(_texture);
        playerRender = _playerRender;
        SetTextureToPlayer(local_jietuTexture);
    }

    void SetTextureToPlayer(Texture2D _texture2D) {
        if( playerRender == null ) {
            return;
        }

        playerRender.material.mainTexture = _texture2D;

        Resources.UnloadUnusedAssets();
        GC.Collect();
    }

    /// <summary>
    /// Texture转换成Texture2D
    /// </summary>
    /// <param name="texture"></param>
    /// <returns></returns>
    private Texture2D TextureToTexture2D(Texture texture) {
        Texture2D texture2D = new Texture2D(texture.width , texture.height , TextureFormat.RGBA32 , false);
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture renderTexture = RenderTexture.GetTemporary(texture.width , texture.height , 32);
        Graphics.Blit(texture , renderTexture);

        RenderTexture.active = renderTexture;
        texture2D.ReadPixels(new Rect(0 , 0 , renderTexture.width , renderTexture.height) , 0 , 0);
        texture2D.Apply();

        RenderTexture.active = currentRT;
        RenderTexture.ReleaseTemporary(renderTexture);

        return texture2D;
    }
}



