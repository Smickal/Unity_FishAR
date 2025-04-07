using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public class Tutorial
{
    public bool IsInteractive;
    public bool IsPanelActivated;
    
    public UITutorialDesc UITutorialDescPrefab;

    [Space(5)] 
    public Transform _CustomParent;
    //public UnityEvent EventToTrigger;
    public Button _ButtonTrigger;
    
    [Space(5)] 
    public UnityEvent CustomEvent;
}

public class TutorialManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public bool IsTutorialActivated = false;
    [Space((5))] 
    [SerializeField] public List<Tutorial> Tutorials;

    [Space((10))] 
    [SerializeField] private GameObject _tutorialPanel;
    [SerializeField] private GameObject _psuedoGO;
    [SerializeField] private Image _tutorialPanelImage;
    
    
    [Space((10))] 
    [SerializeField] private Color _normalPanel;
    [SerializeField] private Color _transparentPanel;

    [Header("Reference")] [Space(5)] 
    [SerializeField] private GameObject _tutorialFishObj;
    [SerializeField] private CameraRayCastManager _cameraRayCastManager;
    [SerializeField] private ObjectSpawnManager _objectSpawnManager;
    [SerializeField] private SaveManager _saveManager;
    

    private RectTransform CurrentPseudoRectTransform;
    private GameObject CurrentPsuedoReplacementGO;
    private Button CurrentbuttonToMove;
    private RectTransform CurrentParentRectTransform;
    private int CurrentSiblingIndex;
    private GameObject TutorialPrefab;
    private UITutorialDesc newCurrentTutorial;

    private int currentTutorialIdx = 0;
    void Start()
    {
        currentTutorialIdx = 0;

        if (IsTutorialActivated && _saveManager.GetSaveData().IsTutorialDone == false)
        {
            CreateTutorial();
            _cameraRayCastManager.ChangeCameraType(ECameraType.ECT_TutorialLearning);
        }
        else
        {
            if(_tutorialFishObj)
                _tutorialFishObj.SetActive(false);
        }
        
        
    }

    public void CreateTutorial()
    {
        _tutorialPanel.SetActive(true);

        MakeNewTutorial();
    }

    private void MakeNewTutorial()
    {
        CurrentbuttonToMove = Tutorials[currentTutorialIdx]._ButtonTrigger;
        
        //Debug.Log(Tutorials[currentTutorialIdx].UITutorialDescPrefab);
        //CREATE extra
        if (Tutorials[currentTutorialIdx].UITutorialDescPrefab)
        {
            newCurrentTutorial = Instantiate(Tutorials[currentTutorialIdx].UITutorialDescPrefab, _tutorialPanel.transform);
            newCurrentTutorial.SetInteractAble(this, Tutorials[currentTutorialIdx].IsInteractive);
        }
        
        //Activate/de-activate Panel
        if (Tutorials[currentTutorialIdx].IsPanelActivated)
        {
            _tutorialPanelImage.color = _normalPanel;
        }
        else
        {
            _tutorialPanelImage.color = _transparentPanel;
        }
        //Debug.Log(_tutorialPanelImage.color);
        
        if (CurrentbuttonToMove)
        {
            CurrentSiblingIndex = CurrentbuttonToMove.transform.GetSiblingIndex();
            CurrentParentRectTransform= CurrentbuttonToMove.transform.parent.gameObject.GetComponent<RectTransform>();
            
            CurrentbuttonToMove.transform.parent = _tutorialPanel.transform;
            CurrentbuttonToMove.onClick.AddListener(NextTutorial);
        }

        if (Tutorials[currentTutorialIdx].CustomEvent.GetPersistentEventCount() > 0)
        {
            Tutorials[currentTutorialIdx].CustomEvent.Invoke();
        }

    }

    public void NextTutorial()
    {
        if (CurrentbuttonToMove)
        {
            CurrentbuttonToMove.onClick.RemoveListener(NextTutorial);
        
            CurrentbuttonToMove.transform.SetParent(CurrentParentRectTransform);
            CurrentbuttonToMove.transform.SetSiblingIndex(CurrentSiblingIndex);
        }

        currentTutorialIdx++;
        
        if (currentTutorialIdx < Tutorials.Count)
        {
            if (newCurrentTutorial)
            {
                Destroy(newCurrentTutorial.gameObject);
                newCurrentTutorial = null;
            }
            
            MakeNewTutorial();
        }
        else
        {
            _tutorialPanel.SetActive(false);
            _objectSpawnManager.ActivateFishes();
            _cameraRayCastManager.ChangeCameraType(ECameraType.ECT_Learning);

            if (_saveManager)
            {
                _saveManager.SaveTutorialDone(true);
            }
        }
        
    }
}
