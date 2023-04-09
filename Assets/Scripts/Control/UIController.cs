using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WeChatWASM;

public class UIController : MonoBehaviour
{
    public Button openCamera;
    public Button takePicture;
    public Button closeCamera;

    private void Start()
    {
        //UI ×¢²áÊÂ¼þ
        openCamera.onClick.AddListener(OpenWebCamDevice);
        takePicture.onClick.AddListener(TakePicture);
        closeCamera.onClick.AddListener(CloseWebCamDevice);
    }

    /// <summary>
    /// Open Media
    /// </summary>
    void OpenWebCamDevice()
    {
      EventCenter.Broadcast(MyEventType.openCamera);
    }

    /// <summary>
    /// ÅÄÕÕ
    /// </summary>
    void TakePicture()
    {
        EventCenter.Broadcast(MyEventType.takePicture); 
        
    }

    /// <summary>
    /// Close Media
    /// </summary>
    void CloseWebCamDevice()
    {
        EventCenter.Broadcast(MyEventType.closeCamera);
    }
}

