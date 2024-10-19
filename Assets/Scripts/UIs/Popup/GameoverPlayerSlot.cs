using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameoverPlayerSlot : MonoBehaviour
{
    public PlayerType playerType;
    [SerializeField] private Text scoreText;
    [SerializeField] private InputField nameInputText;

    public string GetInputName()
    {
        return nameInputText.text;
    }

    public void SetScore(int score)
    {
        scoreText.text = score.ToString("#,##0");
    }
}
