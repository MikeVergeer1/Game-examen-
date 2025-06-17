using TMPro;
using UnityEngine;

public class UpdateUI : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    void Update()
    {
        scoreText.text =  ScoreTrigger.score.ToString() + " :erocS ";
    }
}
