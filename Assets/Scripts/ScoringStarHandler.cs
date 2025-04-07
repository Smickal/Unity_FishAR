using UnityEngine;
using UnityEngine.UI;

public class ScoringStarHandler : MonoBehaviour
{
    [Header("Scoring UI")] 
    [SerializeField] private Image[] Stars;
    [SerializeField] private Sprite _GreyStarSprite;
    [SerializeField] private Sprite _GoldStarSprite;


    public void ResetStar()
    {
        foreach (var star in Stars)
        {
            star.sprite = _GreyStarSprite;
        }
    }

    public void SetStarScore(int Score)
    {
        ResetStar();
        for (int i = 0; i < Score; i++)
        {
            Stars[i].sprite = _GoldStarSprite;
        }
    }
}
