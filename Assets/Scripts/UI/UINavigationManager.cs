using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UINavigationManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("HUD")] 
    [SerializeField] private Button _PhotoButton;
    [SerializeField] private Button _PhotoBookButton;
    
    [Space(5)]
    [SerializeField] private Button _SettingButton;
    [SerializeField] private Button _ResetDataButton;
    [SerializeField] private Button _cancelResetDataButton;

    [Header("GO_Reference")] 
    [SerializeField] private GameObject _PhotoMode_GO;
    [SerializeField] private GameObject _PhotoBook_GO;
    [SerializeField] private GameObject _FishDesc_GO;
    [SerializeField] private GameObject _Setting_GO;
    [SerializeField] private GameObject _ConfirmationResetData_GO;
    [SerializeField] private GameObject _throwAway;

    [Space(5)] 
    [SerializeField] private GameObject[] _AllUINavOBJ;

    [SerializeField] private TransitionAnimationController animationControllerInstance;

    private GameObject currentSelected;

    private void Awake()
    {
        //Subscribe button press
        _PhotoButton.onClick.AddListener(() =>
        {
            OpenAnUI(_PhotoMode_GO);
        });
        
        _PhotoBookButton.onClick.AddListener(() =>
        {
            OpenAnUI(_PhotoBook_GO);
        });
        
        _SettingButton.onClick.AddListener((() =>
        {
            OpenAnUI(_Setting_GO);
        }));
        
        _ResetDataButton.onClick.AddListener((() =>
        {
            Debug.LogWarning("Open Reset comff");
            
            OpenAnUI(_ConfirmationResetData_GO);
            _Setting_GO.SetActive(true);
        }));
        
        _cancelResetDataButton.onClick.AddListener((() =>
        {
            OpenAnUI(_Setting_GO);
        }));
        
        CloseAll();
    }
    

    public void OpenAnUI(GameObject obj)
    {
        foreach (var GO in _AllUINavOBJ)
        {
            if (GO == obj)
            {
                GO.SetActive(true);
            }
            else
            {
                GO.SetActive(false);
            }
        }

        if (currentSelected != obj)
        {
            currentSelected = obj;
        }
        else
        {
            currentSelected.SetActive(false);
            currentSelected = null;
        }
    }

    public void CloseAll()
    {
        OpenAnUI(_throwAway);
    }

    public void BackToMainMenu()
    {
        
        if (animationControllerInstance != null)
        {
            animationControllerInstance.PlayExitAnimation();
            StartCoroutine(DelayEnterNextScene(4f, "AR_MainMenu"));
        }
        else
        {
            SceneManager.LoadScene("AR_Menu");
        }
    }
    
    IEnumerator DelayEnterNextScene(float TimeDelay, string SceneName)
    {
        yield return new WaitForSeconds(TimeDelay);
        SceneManager.LoadScene(SceneName);
    }
}
