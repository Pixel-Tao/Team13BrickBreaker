using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerBoardUI : MonoBehaviour
{
    [SerializeField] private PlayerType playerType;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI lifeText;
    // Start is called before the first frame update
    private void Start()
    {
        GameManager.Instance.OnScoreChanged += ScoreChanged;
    }

    private void ScoreChanged(PlayerType type, int value)
    {
        if (playerType != type) return;
        scoreText.text = value.ToString("#,###");
    }

    private void LifeChanged(PlayerType type, int value)
    {
        if (playerType != type) return;
        lifeText.text = value.ToString("#,###");
    }
}
