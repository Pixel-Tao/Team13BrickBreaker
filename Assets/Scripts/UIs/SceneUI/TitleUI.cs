using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleUI : MonoBehaviour
{

    public void SingleGameStart()
    {
        GameManager.Instance.SetPlayMode(Defines.PlayModeType.Single);
        UIManager.Instance.FadeOut(GameStart);
    }

    public void MultiGameStart()
    {
        GameManager.Instance.SetPlayMode(Defines.PlayModeType.Multi);
        UIManager.Instance.FadeOut(GameStart);
    }

    private void GameStart()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void ShowScoreBoard()
    {
        // TODO : ScoreBoard UI 띄우기
        Debug.Log("Show ScoreBoard");
        UIManager.Instance.ShowPopup<ScoreBoardPopup>();
    }

    public void ShowArchievement()
    {
        // TODO : Archievement UI 띄우기
        Debug.Log("Show Archievement");
        UIManager.Instance.ShowPopup<AchievementPopup>();
    }

    public void ShowSetting()
    {
        // TODO : Setting UI 띄우기
        Debug.Log("Show Setting");
        UIManager.Instance.ShowPopup<SettingPopup>();
    }
}
