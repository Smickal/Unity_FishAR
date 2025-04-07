using UnityEngine;

[CreateAssetMenu(fileName = "TutorialScriptableScript", menuName = "Scriptable Objects/TutorialScriptableScript")]
public class TutorialScriptableScript : ScriptableObject
{
    [Header("Tutorial Type")]
    public bool IsInteractable;
    public bool IsInformative;

    [Space(5)] 
    public GameObject _dataPrefab;
}
