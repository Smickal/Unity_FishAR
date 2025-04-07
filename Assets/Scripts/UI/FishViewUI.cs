using System;
using DistantLands;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class FishViewUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [Space(5)]
    [SerializeField] private GameObject _PanelContainerOBJ;
    [SerializeField] private Transform _fishSpawnPoint;
    [SerializeField] private Button _enlargeButton;
    [SerializeField] private Button _reduceButton;
    [SerializeField] private Button _backButton;
    

    [Space(5)]
    [SerializeField] private FishDescriptionUI _fishDescUi;
    [SerializeField] private GameObject _GameHUD;
    [SerializeField] private PanZoom _canvasCamPanZoom;

    private GameObject CurrentObjView;
    private FishControl CurrentFishPrefab;
    private float MinZoomSize = 0f;
    private float MaxZoomSize = 5f;

    private float CurrentZoomSize = 1f;


    private void Start()
    {
        _enlargeButton.onClick.AddListener(OnEnlargeButtonBtnPresed);
        _reduceButton.onClick.AddListener(OnReduceObjectBtnPressed);
        
        _backButton.onClick.AddListener(ReturnBackButtonPressed);
    }

    public void OpenViewUI(FishControl Fish)
    {
        CurrentFishPrefab = Fish;
        
        FishControl newFish = Instantiate(CurrentFishPrefab, _fishSpawnPoint.position, quaternion.identity, _fishSpawnPoint);
        newFish.gameObject.layer = LayerMask.NameToLayer("UI");
        newFish.gameObject.GetComponent<Fish>().enabled = false;
        newFish.gameObject.GetComponent<FishControl>().enabled = false; 
        newFish.gameObject.SetActive(true);
        
        foreach (var fishComp in newFish.GetComponentsInChildren<MeshRenderer>())
        {
            fishComp.gameObject.layer = LayerMask.NameToLayer("UI");
        }
        
        foreach (var fishComp in newFish.GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            fishComp.gameObject.layer = LayerMask.NameToLayer("UI");
        }
        
        CurrentObjView = newFish.gameObject;
        _PanelContainerOBJ.SetActive(true);

        CurrentObjView.transform.localPosition = Vector3.zero;
        CurrentObjView.gameObject.AddComponent<Spin>();

        MinZoomSize = CurrentFishPrefab.GetFishData().MinScaleOffsetForViewingUI;
        MaxZoomSize = CurrentFishPrefab.GetFishData().MaxScaleOffsetForViewingUI;
        
        
        _canvasCamPanZoom.ActivateZoom(CurrentFishPrefab.GetFishData().MinScaleOffsetForViewingUI, CurrentFishPrefab.GetFishData().MaxScaleOffsetForViewingUI, newFish.transform);
        
        _GameHUD.SetActive(false);
    }

    public void ReturnBackButtonPressed()
    {
        Destroy(CurrentObjView);
        
        _fishDescUi.SetDataForDesc(CurrentFishPrefab);
        
        CurrentObjView = null;
        CurrentFishPrefab = null;

        CurrentZoomSize = 1;
        
        _PanelContainerOBJ.SetActive(false);
        _canvasCamPanZoom.DeactivateZoom();
        
        _GameHUD.SetActive(true);
    }

    public void OnEnlargeButtonBtnPresed()
    {
        CurrentZoomSize = Math.Clamp(CurrentZoomSize + 1, MinZoomSize, MaxZoomSize);

        if (CurrentObjView)
        {
            CurrentObjView.transform.localScale = Vector3.one * CurrentZoomSize;
        }
    }
    
    public void OnReduceObjectBtnPressed()
    {
        CurrentZoomSize = Math.Clamp(CurrentZoomSize - 1, MinZoomSize, MaxZoomSize);

        if (CurrentObjView)
        {
            CurrentObjView.transform.localScale = Vector3.one * CurrentZoomSize;
        }
    }

    
}
