using TMPro;
using UnityEngine;

// UpdateUI.cs
// This script updates the UI to display the current score level in the game.
public class UpdateUI : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    void Update()
    {
        scoreText.text = "LeveL\n" + Score.score.ToString();
    }
}
