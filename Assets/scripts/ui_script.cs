using TMPro;
using UnityEngine;

public class ui_script : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    void Awake()
    {
        if (scoreText == null)
            scoreText = GetComponent<TextMeshProUGUI>();
        ShowScore(0, "");
    }

    public void ShowScore(int score, string lastEvent)
    {
        if (scoreText != null)
            scoreText.text = string.IsNullOrEmpty(lastEvent)
                ? $"Score: {score}"
                : $"Score: {score}\n{lastEvent}";
    }
}
