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
        GameManager.Instance.OnScoreChangedEvent += ScoreChanged;
        GameManager.Instance.OnLifeChangedEvent += LifeChanged;

        ScoreChanged(playerType, GameManager.Instance.GetScore(playerType));
        LifeChanged(playerType, GameManager.Instance.GetLife(playerType));
    }

    private void ScoreChanged(PlayerType type, int value)
    {
        if (playerType != type) return;
        scoreText.text = value.ToString("#,##0");
    }

    private void LifeChanged(PlayerType type, int value)
    {
        if (playerType != type) return;
        lifeText.text = value.ToString("#,##0");
    }
}
