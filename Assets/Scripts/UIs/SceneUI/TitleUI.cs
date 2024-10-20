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
        UIManager.Instance.ShowPopup<ScoreBoardPopup>()?.LoadData();
    }

    public void ShowArchievement()
    {
        UIManager.Instance.ShowPopup<AchievementPopup>()?.LoadData();
    }

    public void ShowSetting()
    {
        UIManager.Instance.ShowPopup<SettingPopup>()?.LoadData();
    }
}
