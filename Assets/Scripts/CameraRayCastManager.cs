using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


public enum ECameraType 
{
    ECT_Learning,
    ECT_Assement,
    ECT_TutorialLearning,
    
    Default_Max
}

public class CameraRayCastManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private ECameraType CameraType;
    
    [Space(3)]
    [SerializeField] private float _CameraTakePhotoCooldown = 0.4f;
    [SerializeField] private Camera MainCamera;
    [SerializeField] private LayerMask _raycastLayer;
    
    
    [Header("Reference")] 
    [SerializeField] private PhotoBookManager _photoBookManager;
    [SerializeField] private FlashCameraController _flashCameraController;
    [SerializeField] private FishAssessmentManager _fishAssessmentManager;
    [SerializeField] private FishControl _tutorialFishControl;
    [SerializeField] private SaveManager _saveManager;
    
    private RaycastHit[] BoxCastHit;
    private List<FishControl> CapturedFishes = new List<FishControl>();
    private List<PhotoPoint> CapturedPhotoPoints = new List<PhotoPoint>();


    private bool isTakingAPicture;
    void Start()
    {
        MainCamera = MainCamera == null ? Camera.main : MainCamera;
        isTakingAPicture = false;
    }
    

    public void TakePhoto()
    {
        if (isTakingAPicture) return;

        isTakingAPicture = true;
        _flashCameraController.PlayFlashAnim();
        StartCoroutine(CooldownEnd());
        
        
        CapturedFishes.Clear();
        CapturedPhotoPoints.Clear();
            
        BoxCastHit = Physics.BoxCastAll(MainCamera.transform.position, Vector3.one * 2f, MainCamera.transform.forward, quaternion.identity, 800000000f, _raycastLayer);
        
        //1. Saves All Fish to captured 
        //2. Turn on all point at capture point
        //3. Compare all point captured and fish
        foreach (var resultHit in BoxCastHit)
        {
            //Save capturedFishes to array
            FishControl HitFish = resultHit.collider.GetComponent<FishControl>();
            if (HitFish && !CapturedFishes.Contains(HitFish))
            {
                CapturedFishes.Add(HitFish);
            }
                
            //Saves all points
            PhotoPoint photoPoint = resultHit.collider.GetComponent<PhotoPoint>();
            if (photoPoint)
            {
                photoPoint.GetComponentInParent<FishControl>().AddPoint();
                if (!CapturedPhotoPoints.Contains(photoPoint))
                {
                    CapturedPhotoPoints.Add(photoPoint);
                }
            }
        }

        switch (CameraType)
        {
            case ECameraType.ECT_Learning:
                HandleLearningCamera();
                break;
            case ECameraType.ECT_TutorialLearning:
                HandleTutorialLearning();
                break;
            case ECameraType.ECT_Assement:
                HandleAssessmentCamera();
                break;

            
            default:
                break;
        }
        
        
    }

    private void HandleLearningCamera()
    {
        if (CapturedFishes != null && CapturedFishes.Count > 0)
        {
            //Calculate ScoreforFish ?
            foreach (FishControl Fish in CapturedFishes)
            {
                Debug.Log(Fish.GetFishType() + " is captured with " + Fish.GetPoint() + " points.");
                if (_photoBookManager)
                {
                    _photoBookManager.AddFishToDatabase(Fish);
                }

                if (_saveManager)
                {
                    _saveManager.SaveFishDataCollected(Fish.GetFishData());
                }
                
                Fish.ResetPoint();
            }
        }
    }

    private void HandleTutorialLearning()
    {
        _photoBookManager.AddFishToDatabase(_tutorialFishControl);
    }
    
    private void HandleAssessmentCamera()
    {
        if (_fishAssessmentManager == null || _fishAssessmentManager.GetFishCurrentTargetForAssessment() == null) return;
        
        if (CapturedFishes != null && CapturedFishes.Count > 0)
        {

            FishControl TargetFish = null;
            
            
            //Checks if found A target in photo
            foreach (FishControl Fish in CapturedFishes)
            {
                if (Fish.GetFishData() == _fishAssessmentManager.GetFishCurrentTargetForAssessment())
                {
                    TargetFish = Fish;
                    break;
                }
            }
            
            //founds a target
            if (TargetFish)
            {
                _fishAssessmentManager.ActivateScoreAssessment(true);
            }
            else
            {
                _fishAssessmentManager.IncreaseClueLevel();
            }
        }
    }
    
    
    IEnumerator CooldownEnd()
    {
        yield return new WaitForSeconds(_CameraTakePhotoCooldown);

        isTakingAPicture = false;
    }


    public void ChangeCameraType(ECameraType Type)
    {
        CameraType = Type;
    }
    
    private void OnDrawGizmos()
    {
        if (BoxCastHit != null  && BoxCastHit.Length > 0)
        {
            foreach (var HitResult in BoxCastHit)
            {
                Gizmos.DrawWireCube(HitResult.point, Vector3.one);
            }
            
        }
        
    }
}
