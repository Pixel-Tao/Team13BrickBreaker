using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameoverPopup : PopupBase
{
    [SerializeField] private GameoverPlayerSlot player1ScoreBoard;
    [SerializeField] private GameoverPlayerSlot player2ScoreBoard;

    [SerializeField] private Text messageText;

    public override void Init()
    {
        player1ScoreBoard.Clear();
        player2ScoreBoard.Clear();

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
        if (IsValid())
        {
            SaveScore();

            UIManager.Instance.FadeOut(() =>
            {
                UIManager.Instance.ClosePopup(this);
                SceneManager.LoadScene("TitleScene");
            });
        }

    }

    private bool IsValid()
    {
        if (player1ScoreBoard.gameObject.activeSelf && IsValid(player1ScoreBoard) == false)
            return false;
        if (player2ScoreBoard.gameObject.activeSelf && IsValid(player2ScoreBoard) == false)
            return false;

        return true;
    }

    private bool IsValid(GameoverPlayerSlot slot)
    {
        string name = player1ScoreBoard.GetInputName();
        if (!string.IsNullOrWhiteSpace(name) && name.Length > 10)
        {
            messageText.text = "이름은 10자 이내로 입력해주세요.";
            return false;
        }

        return true;
    }

    private void SaveScore()
    {
        if (player1ScoreBoard.gameObject.activeSelf)
            SaveScore(player1ScoreBoard);
        if (player2ScoreBoard.gameObject.activeSelf)
            SaveScore(player2ScoreBoard);
    }

    private void SaveScore(GameoverPlayerSlot slot)
    {
        string name = slot.GetInputName();
        SaveScoreDataList data = SaveManager.Instance.Load<SaveScoreDataList>();

        // 입력된 이름 값이 없으면 저장 안함
        if (string.IsNullOrWhiteSpace(name))
            return;

        if (data.scores.Count >= 10)
            data.scores.RemoveAt(data.scores.Count - 1);

        SaveScoreData saveScoreData = new SaveScoreData
        {
            name = name,
            score = GameManager.Instance.GetScore(slot.playerType),
            dateTimeText = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            playerType = slot.playerType
        };

        data.scores.Add(saveScoreData);

        data.scores = data.scores.OrderByDescending(s => s.score).ToList();
        SaveManager.Instance.Save(data);
    }
}
