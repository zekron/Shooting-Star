using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    private Text scoreText;

    private void Awake()
    {
        scoreText = GetComponent<Text>();
    }

    private void Start()
    {
        ScoreManager.Instance.ResetScore();
    }

    public void UpdateText(int score) => scoreText.text = score.ToString();

    public void ScaleText(Vector3 targetScale) => scoreText.rectTransform.localScale = targetScale;
}