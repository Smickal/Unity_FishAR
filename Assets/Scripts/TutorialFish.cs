using UnityEngine;
using UnityEngine.Splines;

public class TutorialFish : MonoBehaviour
{

   [SerializeField] private TutorialManager _tutorialManager;
   [SerializeField] private SplineAnimate _splineAnimate;
   
   public void ActivateNextTutorial()
   {
      gameObject.SetActive(false);
      //_splineAnimate.enabled = true;

   }
}
