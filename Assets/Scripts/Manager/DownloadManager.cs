using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using WeChatWASM;

public class DownloadManager : Singleton<DownloadManager>
{
    public RawImage target;
    public void SetTexture2DFromNet(RawImage target, string path)
    {
        this.target=target;
        StartCoroutine(DownloadImage(path));
    }

    IEnumerator DownloadImage(string path)
    {

        UnityEngine.Networking.UnityWebRequest www = UnityWebRequestTexture.GetTexture(path);
        yield return www.SendWebRequest();

        if (www.isNetworkError||www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture2D myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            //Sprite createSprite = Sprite.Create(myTexture , new Rect(0 , 0 , myTexture.width , myTexture.height) , new Vector2(0 , 0));

            target.texture=myTexture;
            //target.texture=createSprite;
        }
    }

}


/// <summary>
/// µ¥ÀýÄ£°åÀà
/// </summary>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance==null)
            {
                GameObject obj = new GameObject();
                obj.name=typeof(T).Name;
                instance=obj.AddComponent<T>();
                DontDestroyOnLoad(obj);
            }

            return instance;
        }
    }
}