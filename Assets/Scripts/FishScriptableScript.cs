using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FishScriptableScript", 
    menuName = "Scriptable Objects/FishScriptableScript")]
public class FishScriptableScript : ScriptableObject
{
    [SerializeField] public String FishName = "Default_Name";
    [SerializeField] public EFishType FishType = EFishType.RoyalGamma;
    [SerializeField] public Sprite FishImage = null;
    [SerializeField] public String FishSize = "5-7 m";
    
    [Space(5)]
    [TextArea(2, 10)] 
    [SerializeField] public String FishDescription = "Desc";


    [Space(2)] 
    [SerializeField] public float MinScaleOffsetForViewingUI = 0f;
    [SerializeField] public float MaxScaleOffsetForViewingUI = 1f;
    


    [Space(5)]
    [TextArea(2, 5)] 
    [SerializeField] public List<string> FishClues;
}
