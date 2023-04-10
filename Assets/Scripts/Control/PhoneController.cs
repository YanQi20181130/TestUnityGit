using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WeChatWASM;

public class PhoneController : MonoBehaviour
{
    /// <summary>
    /// ����������
    /// </summary>
    public RawImage imgPhoneCameraCanvas;
    /// <summary>
    /// �������ͼ��
    /// </summary>
    private WebCamTexture camTexture;

    [SerializeField,Header("�����߿�ͼ")]
    private RawImage outLine;
    // Start is called before the first frame update
    void Start()
    {
        EventCenter.AddListener(MyEventType.openCamera , OnOpenCamera);
        EventCenter.AddListener(MyEventType.closeCamera , OnCloseCamera);
        EventCenter.AddListener(MyEventType.takePicture , OnTakePhote);

        imgPhoneCameraCanvas.enabled=false;
        outLine.enabled=false;
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
        if (Application.platform==RuntimePlatform.Android ||Application.platform==RuntimePlatform.WindowsEditor)
        {
            //��׿
            StartCoroutine(OpenCameraAndroid());
        }
        else if (Application.platform==RuntimePlatform.WebGLPlayer&&Application.platform!=RuntimePlatform.WindowsEditor)
        {
            //΢��
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
        if (Application.platform==RuntimePlatform.Android||Application.platform==RuntimePlatform.WindowsEditor)
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
        if (Application.platform==RuntimePlatform.Android||Application.platform==RuntimePlatform.WindowsEditor)
        {
            var gettedTex = GetTextureFromWebcamera(camTexture);
            EventCenter.Broadcast<Texture2D>(MyEventType.setTextureToPlayer , gettedTex);
            GlobalDataManager.ChangeState(GlobalDataManager.GameState.cameraPhotoTaked);
            imgPhoneCameraCanvas.enabled=false;
            outLine.enabled=false;
        }
        else
        {

            
        }

    }

    #region ��׿����

    /// <summary>
    /// ��׿�����
    /// </summary>
    /// <returns></returns>
    private IEnumerator OpenCameraAndroid()
    {
        //�ȴ��û��������
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        //����û�������ʣ���ʼ��ȡͼ��        
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            //�Ȼ�ȡ�豸
            WebCamDevice[] device = WebCamTexture.devices;
            Debug.Log("����ͷ�豸���� = "+device.Length);

            if (device.Length<=0)
            {
                Debug.Log("δ��������ͷ�豸");
                yield break;
            }
            string deviceName = device[0].name;
            //Ȼ���ȡͼ��
            camTexture=new WebCamTexture(deviceName);
            //����ȡ��ͼ��ֵ
            imgPhoneCameraCanvas.texture=camTexture;
            //��ʼʵʩ��ȡ
            camTexture.Play();

            imgPhoneCameraCanvas.enabled=true;
            outLine.enabled=true;
            imgPhoneCameraCanvas.gameObject.SetActive(true);
            outLine.gameObject.SetActive(true);
        
            GlobalDataManager.ChangeState(GlobalDataManager.GameState.cameraOpend);

        }
    }

    void StopCameraAndroid()
    {
        //����û�������ʣ���ʼ��ȡͼ��        
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            camTexture.Stop();
            imgPhoneCameraCanvas.enabled=false;

        }
    }

    public Texture2D GetTextureFromWebcamera(WebCamTexture t)
    {
        Texture2D t2d = new Texture2D(t.width , t.height , TextureFormat.RGB24 , true);
        //��WebCamTexture �����ر��浽texture2D��
        t2d.SetPixels(t.GetPixels());
        //t2d.ReadPixels(new Rect(200,200,200,200),0,0,false);
        t2d.Apply();
        return t2d;
    }

    #endregion

    #region ΢��Open Media

    /// <summary>
    /// ������ͷ
    /// </summary>
    public static void OpenWebCamDevice(System.Action<Texture2D> _onComplete)
    {
        Debug.Log("΢��Open camera !");

        if (_onComplete==null) return;

        ChooseImageOption chooseMedia = new ChooseImageOption
        {
            complete=(re) => {
                GlobalDataManager.ChangeState(GlobalDataManager.GameState.cameraOpend);
                Debug.Log($"Opened camera- general complete : "+re.errMsg); } ,
            fail=(re) => { Debug.Log($"Opened camera- general failed : "+re.errMsg); } ,
            sizeType=new string[1] { "compressed" } ,
            count=1 ,
            sourceType=new string[2] { "camera" , "album" } ,
            //���ճɹ���
            success=(result) => {
                // ��ȡͼƬ��ַ
                var tempPath = result.tempFilePaths[0];
                // ��ͼƬ��ַȡͼƬ
                var bytes = WX.GetFileSystemManager().ReadFileSync(tempPath);
                Texture2D texture = new Texture2D(512 , 512);
                texture.LoadImage(bytes);
                if (_onComplete!=null)
                {
                    _onComplete.Invoke(texture);
                }
                else
                {
                    Debug.LogError("δ�ҵ���ɫ");
                }
            }
        };

        WX.ChooseImage(chooseMedia);
    }
    #endregion
}
