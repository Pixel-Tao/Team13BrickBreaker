using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausePlayerSlot : MonoBehaviour
{
    public PlayerType playerType;
    [SerializeField] private Text scoreText;

    public void SetScore(int score)
    {
        scoreText.text = score.ToString("#,##0");
    }
}
