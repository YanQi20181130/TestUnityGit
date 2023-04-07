using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WeChatWASM;

public class UIController : MonoBehaviour
{
    public Button openCamera;

    private void Start()
    {
        openCamera.onClick.AddListener(OpenWebCamDevice);
    }

    /// <summary>
    /// Open Media
    /// </summary>
    void OpenWebCamDevice()
    {
        Utils.OpenWebCamDevice(Utils.GetTextureColor);
    }


}

