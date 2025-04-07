using System;
using UnityEngine;

public class TransitionAnimationController : MonoBehaviour
{   
    
    [Header("Animation")]
    [SerializeField] private Animator _transitionAnimator;

    [Header("Reference")] 
    [SerializeField] private GameObject _container;
    
    
    private int EnterAnimHash = Animator.StringToHash("Enter");
    private int ExitAnimHash = Animator.StringToHash("Exit");

    

    private void Start()
    {
        _container.SetActive(true);
        PlayEnterAnimation();
    }

    public void PlayExitAnimation()
    {
        _container.SetActive(true);
        _transitionAnimator.SetTrigger(ExitAnimHash);
    }

    public void DisableContainer()
    {
        _container.SetActive(false);
    }
    
    public void PlayEnterAnimation()
    {

        _transitionAnimator.SetTrigger(EnterAnimHash);
    }
    
    
    
}
