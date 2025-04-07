using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class FishAssessmentManager : MonoBehaviour
{
    [Header("Properties")] 
    [SerializeField] private int _amountOfClueWillBeDisplayed = 3;
    
    [Header("UI Related")] 
    [SerializeField] private Image _fishImage;
    [SerializeField] private GameObject _deactivateImage_GO;
    [SerializeField] private GameObject _activateImage_GO;
    [SerializeField] private Transform _ObjectiveSpawnTransform;
    [SerializeField] private UIClueText _clueTextPrefab;

    [Header("Reference")] 
    [SerializeField] private ObjectSpawnManager _fishSpawnManager;
    [SerializeField] private ScoringStarHandler _scoringHandler;
    [SerializeField] private ArrrowPointingTowards _3DArrowPointer;
    
    [Header("Assessment UI")] 
    [SerializeField] private GameObject _assessmentUiGO;
    [SerializeField] private Image _assessmentImage;
    [SerializeField] private TMP_Text _assessmentFishNameText;
    [SerializeField] private Button _assessmentBackButton;
    

    [SerializeField] private FishControl[] fishDatas;
    private List<UIClueText> SpawnedClueText = new();
    private int randomChosenFishIndex;

    private FishControl currentFishPrefab;
    private FishScriptableScript currentFishData;
    private FishControl currentFishSelectedInScene;

    private int currentClueLevel = -1;
    private void Start()
    {
        //fishDatas = _fishSpawnManager.GetAllFishPrefab();

        PickARandomFishDataToAssess();
        
        _assessmentBackButton.onClick.AddListener(() =>
        {
            OnAssessmentBackButtonPressed();
        });
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            IncreaseClueLevel();
            Debug.Log("Increasing Level!");
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetClue();
            Debug.Log("Reset Clue!");
        }

        // if (Input.GetMouseButtonDown(0))
        // {
        //     Debug.Log("touch!");
        // }
    }

    private void PickARandomFishDataToAssess()
    {
        //1. Pick random data.
        //2. Pick the fishes from the scene.
        // (if there's no fish) -> go to 1.
        randomChosenFishIndex = Random.Range(0, fishDatas.Length);
        currentFishPrefab = fishDatas[randomChosenFishIndex];
        
        if (currentFishPrefab)
        {
            currentFishData = currentFishPrefab.GetFishData();
        }

        currentFishSelectedInScene = null;
        
        FishControl[] FishesInScene = FindObjectsByType<FishControl>(FindObjectsSortMode.None);
        if (FishesInScene.Length > 0)
        {
            foreach (var Fish in FishesInScene)
            {
                if (Fish.GetFishData() == currentFishData)
                {
                    currentFishSelectedInScene = Fish;
                    break;
                }
            }
        }

        if (currentFishSelectedInScene == null)
        {
            PickARandomFishDataToAssess();
            return;
        }
        
        CreateContextCluesForFish();
    }

    private void CreateContextCluesForFish()
    {
        //Create Text Clues
        //Create Img Clues
        //Level 0 -> Default clue
        //Lvl 1 -> 2 uses text
        //Level 3 -> Image
        //Level 4 -> Point with Arrow
        
        //Create Text

        int maxIndexForClues = currentFishData.FishClues.Count;
        int[] CheckCluesArray = new int[maxIndexForClues];

        int currCountOfClue = 0;
        int clueCount = 1;
        
        while (currCountOfClue < _amountOfClueWillBeDisplayed)
        {
            int randomIndex = Random.Range(0, _amountOfClueWillBeDisplayed);
            if(CheckCluesArray[randomIndex] == 1) continue;
            
            CheckCluesArray[randomIndex] = 1;
            
            UIClueText newClue = Instantiate(_clueTextPrefab, _ObjectiveSpawnTransform);
            string tempString = (clueCount)+ ". " + currentFishData.FishClues[randomIndex];
            newClue.SetRealClue(randomIndex, tempString);
            
            SpawnedClueText.Add(newClue);

            currCountOfClue++;
            clueCount++;
        }
        
        //Create IMG Clues
        _fishImage.sprite = currentFishData.FishImage;
        
        IncreaseClueLevel();
    }

    public void IncreaseClueLevel()
    {
        if (currentClueLevel > _amountOfClueWillBeDisplayed)
        {
            return;
        }
        currentClueLevel++;
        
        if (currentClueLevel < _amountOfClueWillBeDisplayed)
        {
            //Show Text per index in array
            
            SpawnedClueText[currentClueLevel].ActivateRealClue();
        }
        else if(currentClueLevel == _amountOfClueWillBeDisplayed)
        {
            // Show Image as last result
            _deactivateImage_GO.SetActive(false);
            _activateImage_GO.SetActive(true);
        }
        else 
        {
            //Activate Guiding Arrow and fish outlines
            if (currentFishSelectedInScene != null)
            {
                _3DArrowPointer.ActivateArrowTargeting(true, currentFishSelectedInScene.transform);
                currentFishSelectedInScene.ActivateOutline(true);
                
            }
        }
        
        Debug.LogWarning("ClueLevel : " + currentClueLevel);
    }


    public void ResetClue()
    {
        //clean Text Clues
        foreach (var SpawnedTextClue in SpawnedClueText)
        {
            Destroy(SpawnedTextClue.gameObject);
        }
        SpawnedClueText.Clear();
        
        //Clean image
        _deactivateImage_GO.SetActive(true);
        _activateImage_GO.SetActive(false);
        currentClueLevel = -1;
        
        _3DArrowPointer.ActivateArrowTargeting(false, null);
        if (currentFishSelectedInScene)
        {
            currentFishSelectedInScene.ActivateOutline(false);
            currentFishSelectedInScene = null;
        }
        
        PickARandomFishDataToAssess();
        _scoringHandler.ResetStar();
        
    }

    public FishScriptableScript GetFishCurrentTargetForAssessment()
    {
        return currentFishData;
    }

    public void ActivateScoreAssessment(bool value)
    {
        if (value)
        {
            _assessmentImage.sprite = currentFishData.FishImage;
            _assessmentFishNameText.SetText(currentFishData.FishName);
        }
        
        
        
        if (currentClueLevel == 2)
        {
            _scoringHandler.SetStarScore(1);
        }
        else if (currentClueLevel == 1)
        {
            _scoringHandler.SetStarScore(2);
        }
        else if (currentClueLevel == 0)
        {
            _scoringHandler.SetStarScore(3);
        }
        
        _assessmentUiGO.SetActive(value);
    }

    private void OnAssessmentBackButtonPressed()
    {
        ActivateScoreAssessment(false);
        ResetClue();
    }
    
   
}
