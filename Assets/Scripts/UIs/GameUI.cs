using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] private PlayerBoardUI player1;
    [SerializeField] private PlayerBoardUI player2;

    // Start is called before the first frame update
    private void Start()
    {
        GameManager.Instance.OnPlayerJoin += PlayerJoin;
    }

    private void PlayerJoin(PlayerType type)
    {
        if (type == PlayerType.Player1)
        {
            player1.gameObject.SetActive(true);
        }
        else if (type == PlayerType.Player2)
        {
            player2.gameObject.SetActive(true);
        }
    }
}
