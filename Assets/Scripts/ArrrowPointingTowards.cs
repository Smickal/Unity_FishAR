using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public class ArrrowPointingTowards : MonoBehaviour
{
    [SerializeField] private Transform _startLookAtTransform;
    [SerializeField] private Transform _Target;

    [Space(5)] 
    [SerializeField] private GameObject _ArrowContainer;


    private void Start()
    {
        ActivateArrowTargeting(false, null);
    }

    private void LateUpdate()
    {
        if (_startLookAtTransform && _Target)
        {
            Vector3 lookAtDirection = _Target.position - _startLookAtTransform.position;
            transform.rotation = quaternion.LookRotationSafe(lookAtDirection, Vector3.up);
        }
    }

    

    public void ActivateArrowTargeting(bool value, Transform Target)
    {
        _ArrowContainer.SetActive(value);

        if (Target)
        {
            _Target = Target;
        }
        else
        {
            _Target = null;
        }
    }
}
