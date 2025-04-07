using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using File = System.IO.File;

public struct SaveData
{
   public bool IsTutorialDone;
   public bool IsUsingHighContrastFlash;
   public List<FishScriptableScript> FishDataCollected;
}



public class SaveManager : MonoBehaviour
{
   private string KeySaveData = "KeySavedata_Key";
   private SaveData currentSaveData;
   private string saveFilePath;
   
   [SerializeField] private bool IsResetSaveData = false;

   [Header("UI")] 
   [SerializeField] private Button _resetSaveDatabutton;
   
   [Header("Reference")]
   [SerializeField] private ObjectSpawnManager _spawnManager;

   [SerializeField] private UINavigationManager _uiNavManager;
   [SerializeField] private UINavigationMainMenu _mainMenuNavManager;

   private void Awake()
   {
      saveFilePath =  Application.persistentDataPath + "/PlayerData.json";
      
      LoadGameData(); 
   }

   private void Start()
   {
      if (IsResetSaveData)
      {
         ResetSaveData();
      }

      if (_resetSaveDatabutton)
      {
         _resetSaveDatabutton.onClick.AddListener(() =>
         {
            ResetSaveData();
         });
      }
   }

   public void SaveTutorialDone(bool value)
   {
      currentSaveData.IsTutorialDone = value;
      SaveGame();
   }

   public void SaveIsUsingCameraFlash(bool value)
   {
      currentSaveData.IsUsingHighContrastFlash = value;
      
      SaveGame();
   }

   public void SaveFishDataCollected(FishScriptableScript fishControl)
   {
      if (!currentSaveData.FishDataCollected.Contains(fishControl))
      {
         currentSaveData.FishDataCollected.Add(fishControl);
         SaveGame();
      }
   }
   
   private void SaveGame()
   {
      string saveData = JsonUtility.ToJson(currentSaveData);
      File.WriteAllText(saveFilePath, saveData);
   }

   private void LoadGameData()
   {
      if (File.Exists(saveFilePath))
      {
         string LoadSaveData = File.ReadAllText(saveFilePath);
         currentSaveData = JsonUtility.FromJson<SaveData>(LoadSaveData);
         Debug.Log("A SaveFile is Found\n " +
                   "Load Game Completed!");
      }
      else
      {
         currentSaveData = new SaveData();
         currentSaveData.FishDataCollected = new();
         
         Debug.Log("No SaveData detected, \n" +
                   "Creating a new Save file!");
      }
   }

   public SaveData GetSaveData()
   {
      return currentSaveData; 
   }

   public FishScriptableScript IsSavedFishExisted(FishScriptableScript fishControl)
   {
      foreach (var fish in currentSaveData.FishDataCollected)
      {
         if (fish == fishControl)
         {
            return fish;
         }
      }

      return null;
   }

   public void ResetSaveData()
   {
      if (File.Exists(saveFilePath))
      {
         File.Delete(saveFilePath);
         Debug.Log("Deleting Data");
      }
      else
      {
         Debug.Log("N oData To delete");
      }

      if (_uiNavManager)
      {
         _uiNavManager.BackToMainMenu();
      } 
      else if (_mainMenuNavManager && _uiNavManager == null)
      {
         _mainMenuNavManager.BackToMainMenu();  
      }
   }
}
