using TMPro;
using UnityEngine;

public class UIClueText : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private TMP_Text _clueText;

    private string realClue;
    
    public void SetRealClue(int curIdx,  string value)
    {
        realClue = value;
        _clueText.SetText((curIdx+1 )+". ???");
    }

    public void ActivateRealClue()
    {
        _clueText.SetText(realClue);
    }
}
