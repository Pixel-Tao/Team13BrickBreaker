using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PausePopup : PopupBase
{
    [SerializeField] private PausePlayerSlot player1ScoreBoard;
    [SerializeField] private PausePlayerSlot player2ScoreBoard;

    public override void CloseButtonClick()
    {
        UIManager.Instance.Resume();
    }

    public void Init()
    {
        if (GameManager.Instance.PlayModeType == Defines.PlayModeType.Single)
        {
            player1ScoreBoard.gameObject.SetActive(true);
            player2ScoreBoard.gameObject.SetActive(false);
            player1ScoreBoard.SetScore(GameManager.Instance.GetScore(PlayerType.Player1));
        }
        else if (GameManager.Instance.PlayModeType == Defines.PlayModeType.Multi)
        {
            player1ScoreBoard.gameObject.SetActive(true);
            player2ScoreBoard.gameObject.SetActive(true);
            player1ScoreBoard.SetScore(GameManager.Instance.GetScore(PlayerType.Player1));
            player2ScoreBoard.SetScore(GameManager.Instance.GetScore(PlayerType.Player2));
        }
        else
        {
            player1ScoreBoard.gameObject.SetActive(false);
            player2ScoreBoard.gameObject.SetActive(false);
        }
    }

    public void GoTitle()
    {
        UIManager.Instance.Resume();
        GameManager.Instance.PlayerClear();

        UIManager.Instance.FadeOut(() =>
        {
            SceneManager.LoadScene("TitleScene");
        });
    }
}
