﻿/// <summary>
/// By SwanDEV 2017
/// </summary>

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Screenshot Helper example.
/// </summary>
public class ScreenshotDemo : DImageDisplayHandler // Inherits the image handler script, we use this handler to set the captured texture/sprite for displaying on the UI Image in this demo.
{
	[Header("[ Save Settings ]")]
	public FilePathName.SaveFormat saveFormat = FilePathName.SaveFormat.JPG;

    [Header("[ Camera Capture (Camera Capture method example) ]")]
    [Tooltip("Request Texture2D or RenderTexture?")]
    public bool m_RequestTexture2D_CameraCapture = true;
    public float m_Scale = 1f;
    public int m_TaregtWidth, m_TargetHeight;

    [Header("[ Capture Cutout (Capture screen with cutout area Example) ]")]
	public Vector2 iCaptureRegionSize = new Vector2(512, 512);

	[Header("[ Object References ]")]
	public CanvasScaler canvasScaler;
	public UnityEngine.UI.Image displayImage;
	public Text debugText;
	public InputField widthInputField;
	public InputField heightInputField;
	public MeshRenderer cubeMesh;

	public Camera camera1;
	public Camera camera2;
	public Camera camera3;

	private void Start()
	{
        // Check the max. texture size support by the current device GPU 
        Debug.Log("Max. texture size support by the current device GPU: " + SystemInfo.maxTextureSize);

        // Set the anti-aliasing level (1, 2, 4, 8), 1 = disable; 8 = best quality.
        ScreenshotHelper.AntiAliasingLevel = 8;

        ScreenshotHelper.iSetMainOnCapturedCallback((Sprite sprite) =>
        {
            // Your Code to use the captured sprite..
            SetImage(sprite);
            cubeMesh.material.mainTexture = sprite.texture;

            switch (saveFormat)
            {
                case FilePathName.SaveFormat.JPG:
                    SaveAsJPG(sprite.texture);
                    break;
                case FilePathName.SaveFormat.PNG:
                    SaveAsPNG(sprite.texture);
                    break;
                case FilePathName.SaveFormat.GIF:
#if PRO_GIF
                    // Require Pro GIF to save image(s) as GIF.
                    SaveAsGIF(sprite.texture);
#endif
                    break;
            }
        });

        // Show screenshot helper debug message
        ScreenshotHelper.Instance.m_DebugText = debugText;

		OnInputChanges();

		// Check screen orientation for setting canvas resolution
		if(Screen.width > Screen.height)
		{
			canvasScaler.referenceResolution = new Vector2(1920, 1080);
		}
		else
		{
			canvasScaler.referenceResolution = new Vector2(1080, 1920);
		}
	}

	private PointerEventData uiPointerEventData = new PointerEventData(EventSystem.current);
	private List<RaycastResult> uiRaycastResuls = new List<RaycastResult>();
	private bool _isPointedOnUI = false;
	private void Update()
	{
		if(Input.GetMouseButtonDown(0))
		{
			uiPointerEventData.position = Input.mousePosition;
			EventSystem.current.RaycastAll(uiPointerEventData, uiRaycastResuls);
			_isPointedOnUI = (uiRaycastResuls.Count > 0)? true:false;
		}

		if(Input.GetMouseButtonUp(0))
		{
			if(!_isPointedOnUI)
			{
                ScreenshotHelper.iCapture(Input.mousePosition, iCaptureRegionSize, (texture2D) => {
                    // Your Code to use the captured texture..
                    Debug.Log("Touch to capture screen, result image size: " + texture2D.width + " x " + texture2D.height);
                });

            }
		}
	}

	public void OnInputChanges()
	{
		int captureWidth = 512;
		int.TryParse(widthInputField.text, out captureWidth);

		int captureHeight = 512;
		int.TryParse(heightInputField.text, out captureHeight);

		iCaptureRegionSize = new Vector2(captureWidth, captureHeight);
	}

	public void CaptureScreen()
	{
		ScreenshotHelper.iCaptureScreen((texture2D)=>{
            // Your Code to use the captured texture..
            Debug.Log("iCaptureScreen - result image size: " + texture2D.width + " x " + texture2D.height);
        });
	}

    public void CaptureWithCamera(Camera camera)
    {
        if (m_RequestTexture2D_CameraCapture)
        {
            if (m_Scale > 0)
            {
                // Capture with camera, request Texture2D of the specific scale:
                ScreenshotHelper.iCaptureWithCamera(camera, m_Scale, (texture2D) =>
                {
                    // Your Code to use the captured texture..
                    Debug.Log("1. iCaptureWithCamera - result image size: " + texture2D.width + " x " + texture2D.height);
                });
            }
            else
            {
                // Capture with camera, request Texture2D of the specific Width and Height:
                ScreenshotHelper.iCaptureWithCamera(camera, (texture2D) =>
                {
                    // Your Code to use the captured texture..
                    Debug.Log("2. iCaptureWithCamera - result image size: " + texture2D.width + " x " + texture2D.height);
                }, m_TaregtWidth, m_TargetHeight);
            }
        }
        else
        {
            if (m_Scale > 0)
            {
                // Capture with camera, request RenderTexture of the specific scale:
                ScreenshotHelper.iCaptureWithCamera_RenderTexture(camera, m_Scale, (renderTexture) =>
                {
                    // Your Code to use the captured texture..
                    Debug.Log("3. iCaptureWithCamera - result image size: " + renderTexture.width + " x " + renderTexture.height);
                });
            }
            else
            {
                // Capture with camera, request RenderTexture of the specific Width and Height:
                ScreenshotHelper.iCaptureWithCamera_RenderTexture(camera, (renderTexture) =>
                {
                    // Your Code to use the captured texture..
                    Debug.Log("4. iCaptureWithCamera - result image size: " + renderTexture.width + " x " + renderTexture.height);
                }, m_TaregtWidth, m_TargetHeight);
            }
        }
    }

	private void SetImage(Sprite sprite)
	{
        base.Clear(displayImage);               // Clear the previous texture (if any)
		base.SetImage(displayImage, sprite);    // Set the sprite to the UI Image, using the DImageDisplayHandler that inherited by this demo script.
    }

	public void Clear()
	{
		base.Clear(displayImage);
		displayImage.rectTransform.sizeDelta = Vector2.zero;
        ClearScreenshotHelper();
    }

    public void ClearScreenshotHelper()
    {
        ScreenshotHelper.iClear(clearCallback: false, clearTextures: true);
    }

    private void SaveAsJPG(Texture2D tex2D)
	{
		string debugMessage = "Saved_as_JPG_to:_" + new FilePathName().SaveTextureAs(tex2D, FilePathName.SaveFormat.JPG);
		ScreenshotHelper.Instance.UpdateDebugText(debugMessage);
	}

	private void SaveAsPNG(Texture2D tex2D)
	{
		string debugMessage = "Saved_as_PNG_to:_" + new FilePathName().SaveTextureAs(tex2D, FilePathName.SaveFormat.PNG);
		ScreenshotHelper.Instance.UpdateDebugText(debugMessage);
	}

#if PRO_GIF
    private void SaveAsGIF(Texture2D tex2D)
    {
        string debugMessage = "Saved as GIF to: " + new FilePathName().SaveTextureAs(tex2D, FilePathName.SaveFormat.GIF);
        ScreenshotHelper.Instance.UpdateDebugText(debugMessage);
    }
#endif

	public void UnRegRenderCameras()
	{
		ScreenshotHelper.iUnRegisterAllRenderCameras();
	}

	public void MoreAssets()
	{
		Application.OpenURL("https://www.swanob2.com/assets");
	}
}
