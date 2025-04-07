using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UITutorialDesc : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private Button _tutorialDescBtn;

    public void SetInteractAble(TutorialManager Manager, bool isInteractive)
    {
        if (!isInteractive) return;
            
        _tutorialDescBtn.onClick.AddListener(() =>
        {
            Manager.NextTutorial();
        });
    }
}
