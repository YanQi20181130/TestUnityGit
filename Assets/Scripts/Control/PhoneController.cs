using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WeChatWASM;

public class PhoneController : MonoBehaviour
{
    /// <summary>
    /// 相机画面呈现
    /// </summary>
    public RawImage imgPhoneCameraCanvas;
    /// <summary>
    /// 相机画面图像
    /// </summary>
    private WebCamTexture camTexture;
    // Start is called before the first frame update
    void Start()
    {
        EventCenter.AddListener(MyEventType.openCamera , OnOpenCamera);
        EventCenter.AddListener(MyEventType.closeCamera , OnCloseCamera);
        EventCenter.AddListener(MyEventType.takePicture , OnTakePhote);
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener(MyEventType.openCamera , OnOpenCamera);
        EventCenter.RemoveListener(MyEventType.closeCamera , OnCloseCamera);
        EventCenter.RemoveListener(MyEventType.takePicture , OnTakePhote);

        camTexture=null;
    }
    /// <summary>
    /// Open Media
    /// </summary>
    void OnOpenCamera()
    {
        if (Application.platform==RuntimePlatform.Android)
        {
            //安卓
            StartCoroutine(OpenCameraAndroid());
        }
        else
        {
            //微信
            OpenWebCamDevice((tex) => {
                EventCenter.Broadcast<Texture2D>(MyEventType.setTextureToPlayer , tex);
            });
        }
    }

    /// <summary>
    /// Close Media
    /// </summary>
    void OnCloseCamera()
    {
        if (Application.platform==RuntimePlatform.Android)
        {
            StopCameraAndroid();
        }
        else
        {

            //Utils.OnOpenCamera(Utils.GetTextureColor);
        }
    }

    void OnTakePhote()
    {
        if (Application.platform==RuntimePlatform.Android)
        {
            var gettedTex = GetTextureFromWebcamera(camTexture);
            EventCenter.Broadcast<Texture2D>(MyEventType.setTextureToPlayer , gettedTex);
        }
        else
        {

            
        }
    }

    #region 安卓拍照

    /// <summary>
    /// 安卓打卡相机
    /// </summary>
    /// <returns></returns>
    private IEnumerator OpenCameraAndroid()
    {
        //等待用户允许访问
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        //如果用户允许访问，开始获取图像        
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            //先获取设备
            WebCamDevice[] device = WebCamTexture.devices;

            string deviceName = device[0].name;
            //然后获取图像
            camTexture=new WebCamTexture(deviceName);
            //将获取的图像赋值
            imgPhoneCameraCanvas.texture=camTexture;
            //开始实施获取
            camTexture.Play();

        }
    }

    void StopCameraAndroid()
    {
        //如果用户允许访问，开始获取图像        
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            camTexture.Stop();
        }
    }

    public Texture2D GetTextureFromWebcamera(WebCamTexture t)
    {
        Texture2D t2d = new Texture2D(t.width , t.height , TextureFormat.RGB24 , true);
        //将WebCamTexture 的像素保存到texture2D中
        t2d.SetPixels(t.GetPixels());
        //t2d.ReadPixels(new Rect(200,200,200,200),0,0,false);
        t2d.Apply();
        return t2d;
    }

    #endregion

    #region 微信Open Media

    /// <summary>
    /// 打开摄像头
    /// </summary>
    public static void OpenWebCamDevice(System.Action<Texture2D> _onComplete)
    {
        Debug.Log("Openning camera !");

        if (_onComplete==null) return;

        ChooseImageOption chooseMedia = new ChooseImageOption
        {
            complete=(re) => { Debug.Log($"Opened camera- general complete : "+re.errMsg); } ,
            fail=(re) => { Debug.Log($"Opened camera- general failed : "+re.errMsg); } ,
            sizeType=new string[1] { "compressed" } ,
            count=1 ,
            sourceType=new string[2] { "camera" , "album" } ,
            //拍照成功后
            success=(result) => {
                // 获取图片地址
                var tempPath = result.tempFilePaths[0];
                // 从图片地址取图片
                var bytes = WX.GetFileSystemManager().ReadFileSync(tempPath);
                Texture2D texture = new Texture2D(512 , 512);
                texture.LoadImage(bytes);
                if (_onComplete!=null)
                {
                    _onComplete.Invoke(texture);
                }
                else
                {
                    Debug.LogError("未找到角色");
                }
            }
        };

        WX.ChooseImage(chooseMedia);
    }
    #endregion
}
