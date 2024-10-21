using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingPopup : PopupBase
{
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Dropdown titleBgmDropdown;
    [SerializeField] private Dropdown gameBgmDropdown;

    private AudioClipType[] bgmTypes =
    {
        AudioClipType.bgm1,
        AudioClipType.bgm2,
        AudioClipType.bgm3,
        AudioClipType.bgm4,
        AudioClipType.bgm5,
    };

    public override void CloseButtonClick()
    {
        SaveData();
        base.CloseButtonClick();
        AudioClipType selectedType = Enum.Parse<AudioClipType>(titleBgmDropdown.captionText.text);
        AudioManager.Instance.PlayBgm(selectedType);
    }

    private void Start()
    {
        bgmSlider.value = AudioManager.Instance.Bgm.volume;
        sfxSlider.value = AudioManager.Instance.Sfx.volume;

        SetDropdownItems();
    }

    private void SetDropdownItems()
    {
        titleBgmDropdown.options.Clear();
        gameBgmDropdown.options.Clear();

        for (int i = 0; i < bgmTypes.Length; i++)
        {
            titleBgmDropdown.options.Add(new Dropdown.OptionData(bgmTypes[i].ToString()));
            gameBgmDropdown.options.Add(new Dropdown.OptionData(bgmTypes[i].ToString()));
        }
    }

    private void SaveData()
    {
        SaveSettingData saveData = new SaveSettingData();
        saveData.bgmVolume = bgmSlider.value;
        saveData.sfxVolume = sfxSlider.value;
        saveData.titleBGM = Enum.Parse<AudioClipType>(titleBgmDropdown.captionText.text);
        saveData.gameBGM = Enum.Parse<AudioClipType>(gameBgmDropdown.captionText.text);
        SaveManager.Instance.Save(saveData);
    }

    public override void LoadData()
    {
        SaveSettingData settingData = SaveManager.Instance.Load<SaveSettingData>();
        bgmSlider.value = settingData.bgmVolume;
        sfxSlider.value = settingData.sfxVolume;
        SetBGMVolume(settingData.bgmVolume);
        SetSFXVolume(settingData.sfxVolume);

        titleBgmDropdown.value = Array.IndexOf(bgmTypes, settingData.titleBGM);
        gameBgmDropdown.value = Array.IndexOf(bgmTypes, settingData.gameBGM);
    }

    public void OnBgmVolumeChanged()
    {
        SetBGMVolume(bgmSlider.value);
    }

    public void OnSfxVolumeChanged()
    {
        SetSFXVolume(sfxSlider.value);
    }

    private void SetBGMVolume(float volume)
    {
        AudioManager.Instance.VolumeBgm(volume);
    }

    private void SetSFXVolume(float volume)
    {
        AudioManager.Instance.VolumeSfx(volume);
    }
}
