using System;
using UnityEngine;
using UnityEngine.Splines;
using Random = UnityEngine.Random;

public class FishControl : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private FishScriptableScript FishData;

    [SerializeField] private SplineAnimate splineAnimate;
    [SerializeField] private PhotoPoint[] PhotoPoints;
    
    [Space(5)]
    [SerializeField] private bool IsUsingManager = true;
    private SplineContainer CurrentUseSpline;


    [Header("Reference")] 
    [SerializeField] private Outline _outline;
    
    private SplineContainer[] Splines;
    private int Point;
    void Start()
    {
        splineAnimate.PlayOnAwake = false;
        ActivateOutline(false);
    }

  
    private void Update()
    {
        if (IsUsingManager)
        {
            if (splineAnimate.IsPlaying)
            {
                if (splineAnimate.NormalizedTime >= .98f)
                {   
                    splineAnimate.Pause();
                    GetRandomSplineRoute();
                    //Debug.Log("1");
                    PlaySplineAnimate();
                }
            }
        }
        
    }
    
    public void SetSplineForAnimate(SplineContainer[] splineContainer)
    {
        Splines = splineContainer;
        GetRandomSplineRoute();
    }

    private void GetRandomSplineRoute()
    {
        if (Splines == null)
        {
            return;
        }
        
        if (Splines.Length > 0)
        {
            CurrentUseSpline = Splines[Random.Range(0, Splines.Length)];
            splineAnimate.Container = CurrentUseSpline;
        }
    }

    public void PlaySplineAnimate()
    {
        if (!splineAnimate && !CurrentUseSpline)
        {
            Debug.Log("Failed to get SplineAnimate/Container");
            return;
        }

        float randomOffset = Random.Range(0f, 0.1f);
        splineAnimate.StartOffset = randomOffset;
        
        if (!splineAnimate.IsPlaying)
        {
            splineAnimate.Restart(true);
        }
    }

    public void AddPoint()
    {
        Point++;
    }

    public void ResetPoint()
    {
        Point = 0;
    }

    public int GetPoint()
    {
        return Point;
    }

    public EFishType GetFishType()
    {
        return FishData.FishType;
    }

    public FishScriptableScript GetFishData()
    {
        return FishData;
    }

    public void ActivateOutline(bool value)
    {
        if (_outline)
        {
            _outline.enabled = value;
        }
    }
}
