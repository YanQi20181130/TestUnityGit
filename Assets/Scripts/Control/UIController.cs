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
        closeCamera.gameObject.SetActive(false);

        EventCenter.AddListener<GlobalDataManager.GameState>(MyEventType.stateChanged , ChangeUIState);
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

    private void ChangeUIState(GlobalDataManager.GameState state)
    {
        switch (state) {
            case GlobalDataManager.GameState.cameraOpend:
            openCamera.gameObject.SetActive(false);
            takePicture.gameObject.SetActive(true);
            break;

            case GlobalDataManager.GameState.cameraPhotoTaked:
            openCamera.gameObject.SetActive(true);
            takePicture.gameObject.SetActive(false);
            CloseWebCamDevice();
            break;  
        
            case GlobalDataManager.GameState.None:
            openCamera.gameObject.SetActive(true);
            takePicture.gameObject.SetActive(false);
            closeCamera.gameObject.SetActive(false);
            break;
        }
    }
}

