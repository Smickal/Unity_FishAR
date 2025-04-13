using System;
using UnityEngine;
using UnityEngine.UI;

public class PhotoRenderManager : MonoBehaviour
{
    [SerializeField] private GameObject _cameraRenderContainer;
    [SerializeField] private GameObject _UIPopUpContainer;
    [SerializeField] private Button _BackButton;


    private void Start()
    {
        _BackButton.onClick.AddListener(() =>
        {
            ClosePopUpPicture();
        });
    }

    public void TriggerOpenPopUpPicture()
    {
        // _cameraRenderContainer.SetActive(true);
        // _cameraRenderContainer.GetComponent<Camera>().Render();
        // _cameraRenderContainer.SetActive(false);
        
        _UIPopUpContainer.SetActive(true);
    }

    private void ClosePopUpPicture()
    {
        _UIPopUpContainer.SetActive(false);
    }
    
}
