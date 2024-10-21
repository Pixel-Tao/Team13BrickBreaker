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

    private void Awake()
    {
        GameManager.Instance.OnScoreChangedEvent += ScoreChanged;
        GameManager.Instance.OnLifeChangedEvent += LifeChanged;
    }

    private void Start()
    {
        scoreText.text = "0";
        lifeText.text = "0";
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnScoreChangedEvent -= ScoreChanged;
        GameManager.Instance.OnLifeChangedEvent -= LifeChanged;
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
