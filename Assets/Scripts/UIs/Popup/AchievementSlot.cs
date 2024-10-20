using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementSlot : MonoBehaviour
{
    [SerializeField] private Text titleText;
    [SerializeField] private Text descriptionText;
    [SerializeField] private Sprite defaultRewardImage;
    [SerializeField] private Sprite lockedSprite;
    [SerializeField] private Image rewardImage;

    public StageData StageData { get; private set; }

    public void SetData(StageData data)
    {
        this.StageData = data;
    }

    public void UnlockAchievement()
    {
        if (StageData == null) return;
        titleText.text = StageData.title;
        descriptionText.text = StageData.description;
        if (StageData.rewardSprite == null)
            rewardImage.sprite = defaultRewardImage;
        else
            rewardImage.sprite = StageData.rewardSprite;
    }

    public void LockAchievement()
    {
        if (StageData == null) return;
        titleText.text = StageData.title;
        descriptionText.text = "???";
        rewardImage.sprite = lockedSprite;
    }
}
