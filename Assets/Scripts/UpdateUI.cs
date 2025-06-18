using TMPro;
using UnityEngine;

public class UpdateUI : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    void Update()
    {
        scoreText.text =  Score.score.ToString() + " :erocS ";
    }
}
