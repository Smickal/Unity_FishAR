using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UINavigationMainMenu : MonoBehaviour
{
    [Header("Buttons")] 
    [SerializeField] private Button _LearningButton;
    [SerializeField] private Button _AssessmentButton;
    [SerializeField] private Button _SettingButton;
    [SerializeField] private Button _QuitButton;
    
    //settingsContainer
    [Space(3)]
    [SerializeField] private Button _BackFromSettingButton;
    [SerializeField] private Button _ResetDataButton;
    //Reset inside
    [Space(3)]
    [SerializeField] private Button _ResetDataYesButton;
    [SerializeField] private Button _ResetDataNoButton;
    
    
    [Header("Containers")] 
    [SerializeField] private GameObject _MainMenuContainer;
    [SerializeField] private GameObject _SettingContainer;
    [SerializeField] private GameObject _PopUpResetDataConfirmationContainer;
    
    [Space(10)]
    [SerializeField] private GameObject[] _allContainer;

    [Space(4)]
    [SerializeField] private TransitionAnimationController animationControllerInstance;
    private void Awake()
    {
        _LearningButton.onClick.AddListener(OnLearningButtonPressed);
        _AssessmentButton.onClick.AddListener(OnAssessmentButtonPressed);
        _SettingButton.onClick.AddListener(OnSettingButtonPressed);
        _BackFromSettingButton.onClick.AddListener(OnBackFromSettingButtonPressed);
        _ResetDataButton.onClick.AddListener(OnResetDataButtonPressed);
        _ResetDataYesButton.onClick.AddListener(OnYesResetDaraButtonPressed);
        _ResetDataNoButton.onClick.AddListener(OnNoResetDaraButtonPressed);
        
        _QuitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
        
    }

    private void Start()
    {
         OpenAContainer(_MainMenuContainer);
    }

    private void OnLearningButtonPressed()
    {
        //ToDo: Open learning Scene
        //Open Learning Scene
        if (animationControllerInstance != null)
        {
            animationControllerInstance.PlayExitAnimation();
            StartCoroutine(DelayEnterNextScene(4f, "AR_Learning"));
        }
        else
        {
            SceneManager.LoadScene("AR_Learning");
        }
    }

    private void OnAssessmentButtonPressed()
    {
        //ToDo: Open Assessment Scene
        if (animationControllerInstance != null)
        {
            animationControllerInstance.PlayExitAnimation();
            StartCoroutine(DelayEnterNextScene(4f, "AR_Assessment"));
        }
        else
        {
            SceneManager.LoadScene("AR_Learning");
        }
    }

    private void OnSettingButtonPressed()
    {
        OpenAContainer(_SettingContainer);
    }

    private void OnBackFromSettingButtonPressed()
    {
        OpenAContainer(_MainMenuContainer);
    }

    private void OnResetDataButtonPressed()
    {
        _PopUpResetDataConfirmationContainer.SetActive(true);
    }

    private void OnNoResetDaraButtonPressed()
    {
        _PopUpResetDataConfirmationContainer.SetActive(false);
    }
    
    private void OnYesResetDaraButtonPressed()
    {
        //TODO : RESET DATA HERE!
    }
    public void OpenAContainer(GameObject gameObject)
    {
        foreach (var go in _allContainer)
        {
            if (go == gameObject)
            {
                go.SetActive(true);
            }
            else
            {
                go.SetActive(false);
            }
        }
    }

    IEnumerator DelayEnterNextScene(float TimeDelay, string SceneName)
    {
        yield return new WaitForSeconds(TimeDelay);
        SceneManager.LoadScene(SceneName);
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
}
