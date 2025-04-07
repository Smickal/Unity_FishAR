using System;
using System.Collections.Generic;
using UnityEngine;

public class PhotoBookManager : MonoBehaviour
{


    [Header("Reference")] 
    [SerializeField] private Transform _FishUISpawnPoint;
    [SerializeField] private SaveManager _saveManager;
    [SerializeField] private ObjectSpawnManager _objectSpawnManager;
    
    [Header("Prefabs")] 
    [SerializeField] private FishPhotoContainerUI _containerUI;
    

    [SerializeField] private List<FishPhotoContainerUI> spawnedFishUI;

    [SerializeField] private FishDescriptionUI _fishDescUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    

    void Start()
    {
        foreach (var FishUI in spawnedFishUI)
        {
            FishUI.SetFishDescUI(_fishDescUI);

            if (_saveManager)
            {
                FishScriptableScript fishControlSaved = _saveManager.IsSavedFishExisted(FishUI.GetFishData());

                if (fishControlSaved)
                {
                    FishUI.ActivatePhotoContainer(true);
                    FishUI.SetFishPrefab(_objectSpawnManager.GetFishPrefab(fishControlSaved));
                }
            }
        }
        
    }
    

    public void AddFishToDatabase(FishControl fish)
    {
        foreach (var FishUI in spawnedFishUI)
        {
            
            if (FishUI.GetFishData() == fish.GetFishData())
            {
                FishUI.ActivatePhotoContainer(true);
                FishUI.SetFishPrefab(fish);
            }
        }
    }
}
