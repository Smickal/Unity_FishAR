using System;
using DistantLands;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class FishDescriptionUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _fishNameText;
    [SerializeField] private TMP_Text _FishSizeText;
    [SerializeField] private TMP_Text _FishDescText;
    [SerializeField] private Image _fishImage;
    [SerializeField] private Button _backButton;
    [SerializeField] private Button _viewFishButton;
    
    [Space(5)] 
    [SerializeField] private GameObject _panelContainer;
    [SerializeField] private Transform _FishSpawnLocation;


    [Space(3)] 
    [SerializeField] private UINavigationManager _uiNavigationManager;
    [SerializeField] private FishViewUI _fishViewUI;
    [SerializeField] private PanZoom _canvasCameraPanZoom;

    [SerializeField] private GameObject _PhotoBookOBJ;
    private FishControl fishPrefab;
    private FishScriptableScript fishData;
    
    private void Start()
    {
        _backButton.onClick.AddListener(() =>
        {
            _uiNavigationManager.CloseAll();
            _uiNavigationManager.OpenAnUI(_PhotoBookOBJ);
            fishPrefab = null;
        });
        
        _viewFishButton.onClick.AddListener(() =>
        {
            _fishViewUI.OpenViewUI(fishPrefab);
            _uiNavigationManager.CloseAll();
        });
    }

    public void SetDataForDesc(FishControl fishPrefab)
    {
        if(fishPrefab == null) return;
        
        Debug.Log("showfish!");
        
        this.fishPrefab = fishPrefab;
        fishData = fishPrefab.GetFishData();
        
        _fishNameText.SetText(fishData.FishName);
        _FishSizeText.SetText(fishData.FishSize);
        _FishDescText.SetText(fishData.FishDescription);
        if (fishData.FishImage)
        {
            _fishImage.sprite = fishData.FishImage;
        }

        // FishControl newFish = Instantiate(this.fishPrefab, _FishSpawnLocation.position, quaternion.identity, _FishSpawnLocation);
        // newFish.gameObject.layer = LayerMask.NameToLayer("UI");
        // newFish.gameObject.GetComponent<Fish>().enabled = false;
        // newFish.gameObject.GetComponent<FishControl>().enabled = false; 

        // foreach (var fishComp in newFish.GetComponentsInChildren<MeshRenderer>())
        // {
        //     fishComp.gameObject.layer = LayerMask.NameToLayer("UI");
        // }
        //
        // CurrentFish = newFish.gameObject;
        
        _panelContainer.SetActive(true);
        _PhotoBookOBJ.SetActive(false);
    }

}
