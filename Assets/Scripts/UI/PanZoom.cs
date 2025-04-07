using UnityEngine;

public class PanZoom : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    private Vector3 touchStart;

    public float zoomOutMin = 3;
    public float zoomOutMax = 14;

    private bool isZoomActivated = false;
    private Transform currentViewTransform;

    // Update is called once per frame
    void Update()
    {
        


        if (Input.touchCount == 2 && isZoomActivated)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector3 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector3 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float curMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = curMagnitude - prevMagnitude;
            
            Zoom(difference * 0.01f);
        }

    }

    void Zoom(float increment)
    {
        //_camera.fieldOfView = Mathf.Clamp(_camera.fieldOfView - increment, zoomOutMin, zoomOutMax);

        float currentScale = Mathf.Clamp(currentViewTransform.localScale.x + increment, zoomOutMin, zoomOutMax);
        currentViewTransform.localScale = Vector3.one * currentScale;
    }
    
    //use field of view
    public void ActivateZoom(float minZoom, float maxZoom, Transform transform)
    {
        zoomOutMin = minZoom;
        zoomOutMax = maxZoom;

        currentViewTransform = transform;
        
        //_camera.orthographicSize= Mathf.Clamp(_camera.fieldOfView , zoomOutMin, zoomOutMax);

        isZoomActivated = true;
        
        float currentScale = Mathf.Clamp(currentViewTransform.localScale.x , zoomOutMin, zoomOutMax);
        currentViewTransform.localScale = Vector3.one * currentScale;
    }

    public void DeactivateZoom()
    {
        isZoomActivated = false;
        //_camera.orthographicSize = 14.4f;
    }
}
