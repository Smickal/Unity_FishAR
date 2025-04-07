using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class FlashCameraController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Animator _flashAnimator;
    [SerializeField] private Image _FlashImage;
    [SerializeField] private GameObject _flashContainer;

    [Header("UI")] 
    [Space(2)] 
    [SerializeField] private Button _activationButton;
    [SerializeField] private Image _settingImage;
    [SerializeField] private Sprite _checkMarkSprite;
    [SerializeField] private Sprite _crossMarkSprite;

    [Header("Reference")] 
    [Space(2)] 
    [SerializeField] private SaveManager _saveManager;
    

    
    private int AnimatorTriggerHash = Animator.StringToHash("Flash");

    private bool isFlashing = false;
    private bool isHighContrast = true;
    private void Start()
    {
        if (_flashContainer.activeSelf)
        {
            _flashContainer.SetActive(false);
        }

        if (_activationButton)
        {
            _activationButton.onClick.AddListener(() =>
            {
                SetFlipFlopHighContrast();
            });
        }

        if (!_saveManager.GetSaveData().IsUsingHighContrastFlash)
        {
            SetFlipFlopHighContrast();
        }
    }

    public void PlayFlashAnim()
    {
        if (_flashAnimator == null || _flashContainer == null || isFlashing) return;
        
        //Debug.Log("Trigger Flash!");
        _flashContainer.SetActive(true);
        _flashAnimator.SetTrigger(AnimatorTriggerHash);
        isFlashing = true;
            
        StartCoroutine(OnTimerEndTrig());
    }
    
    IEnumerator OnTimerEndTrig()
    {
        yield return new WaitForSeconds(0.2f);
        _flashContainer.SetActive(false);
        isFlashing = false;
    }

    public void SetFlipFlopHighContrast()
    {
        isHighContrast = !isHighContrast;
        if (isHighContrast)
        {
            _FlashImage.color = Color.white;
            _settingImage.sprite = _checkMarkSprite;
        }
        else
        {
            _FlashImage.color = Color.black;
            _settingImage.sprite = _crossMarkSprite;
        }

        if (_saveManager)
        {
            _saveManager.SaveIsUsingCameraFlash(isHighContrast);
        }
    }
}
