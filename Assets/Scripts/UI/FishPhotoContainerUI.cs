using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FishPhotoContainerUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [Header("UI_Reference")] 
    [SerializeField] private TMP_Text _FishNameText;
    [SerializeField] private Image _FishImage;
    [SerializeField] private Button _FishBtn;
    
    
    [Header("GO_Reference")] 
    [SerializeField] private GameObject _ActivateContainer;
    [SerializeField] private GameObject _DeactivateContainer;
    [SerializeField] private GameObject _photoBookOBJ;
    
    [SerializeField] private FishScriptableScript fishData;
    private bool isActivated;
    private FishControl CurrentFishPrefab;
    private FishDescriptionUI FishDescriptionUI;
    
    private void Start()
    {

        if (fishData != null)
        {
            SetFishData(fishData);
        }
        
        _FishBtn.onClick.AddListener(() =>
        {
            FishDescriptionUI.SetDataForDesc(CurrentFishPrefab);
        });
        
        //ActivatePhotoContainer(false);
    }

    public void SetFishPrefab(FishControl FishPrefab)
    {
        CurrentFishPrefab = FishPrefab;
    }
    
    public void SetFishDescUI(FishDescriptionUI UI)
    {
        FishDescriptionUI = UI;
    }

    public void ActivatePhotoContainer(bool isActivate)
    {
        isActivated = isActivate;
        _ActivateContainer.SetActive(isActivate);
        _FishBtn.enabled = isActivate;
        
        _DeactivateContainer.SetActive(!isActivate);
    }

    private void OnEnable()
    {
        _ActivateContainer.SetActive(isActivated);
        _DeactivateContainer.SetActive(!isActivated);
    }

    public void SetFishData(FishScriptableScript Data)
    {
        fishData = Data;

        if (_FishNameText)
        {
            _FishNameText.SetText(fishData.FishName);
        }

        if (_FishImage && fishData.FishImage)
        {
            _FishImage.sprite = fishData.FishImage;
        }
        
    }

    public FishScriptableScript GetFishData()
    {
        return fishData;
    }

    public bool IsActivated()
    {
        return isActivated;
    }
}
