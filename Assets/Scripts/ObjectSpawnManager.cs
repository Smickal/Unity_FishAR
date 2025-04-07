using System;
using System.Collections;
using System.Collections.Generic;
using DistantLands;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Splines;
using Random = UnityEngine.Random;

public class ObjectSpawnManager : MonoBehaviour
{
    [SerializeField] private int MaxSpawnedFish = 10;
    [SerializeField] private int DelayBetweenSpawn = 1;
    
    [FormerlySerializedAs("FishPrefabs")]
    [Header("Fishes")]
    [SerializeField] private FishControl[] FishesToSpawnPrefabs;
    [SerializeField]private FishControl[] AllFishDatabase;

    
    [Space(3)] 
    [Header("Splines")] 
    [SerializeField]
    private SplineContainer[] SplineContainers;

    [Space(3)] 
    [Header("Reference")] 
    [SerializeField] private TutorialManager _tutorialManager;
    [SerializeField] private GameObject _FishFlock_OBJ;
    [SerializeField] private GameObject _ExtraFishes_OBJ;
    [SerializeField] private GameObject _fishTutorial_obj;
    [SerializeField] private SaveManager _saveManager;
    
    private List<FishControl> SpawnedFish;
    private int CurrentSpawnFish = 0;
    

    void Start()
    {
        if (_tutorialManager && _tutorialManager.IsTutorialActivated)
        {
            
        }
        else
        {
            ActivateFishes();
        }

        if (_saveManager.GetSaveData().IsTutorialDone)
        {
            ActivateFishes();
        }
        
    }

    public void ActivateTutorialFish()
    {
        _fishTutorial_obj.SetActive(true);
    }
    
    public void ActivateFishes()
    {
        StartCoroutine(SpawnFish());
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
        _ExtraFishes_OBJ.SetActive(true);
        _FishFlock_OBJ.SetActive(true);
    }

    IEnumerator SpawnFish()
    {
        FishControl NewFish = Instantiate(FishesToSpawnPrefabs[Random.Range(0, FishesToSpawnPrefabs.Length)]);
        NewFish.GetComponent<Fish>().enabled = false;
        NewFish.SetSplineForAnimate(SplineContainers);
        NewFish.PlaySplineAnimate();
        
        CurrentSpawnFish++;
        
        yield return new WaitForSeconds(Random.Range(0.1f, DelayBetweenSpawn));
        if (CurrentSpawnFish >= MaxSpawnedFish)
        {
            StopCoroutine(SpawnFish());
        }
        else
        {
            StartCoroutine(SpawnFish());
        }
    }

    public FishControl[] GetAllFishPrefab()
    {
        if (FishesToSpawnPrefabs.Length == 0) return null;

        return FishesToSpawnPrefabs;
    }

    public FishControl GetFishPrefab(FishScriptableScript fishData)
    {
        foreach (var fish in AllFishDatabase)
        {
            if (fish.GetFishData() == fishData)
            {
                return fish;
            }
        }

        return null;
    }
}
