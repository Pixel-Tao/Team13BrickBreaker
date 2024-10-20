using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPopup : PopupBase
{
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    public override void CloseButtonClick()
    {
        SaveData();
        base.CloseButtonClick();
    }

    private void Start()
    {
        bgmSlider.value = AudioManager.Instance.Bgm.volume;
        sfxSlider.value = AudioManager.Instance.Sfx.volume;
    }

    private void SaveData()
    {
        SaveSettingData saveData = new SaveSettingData();
        saveData.bgmVolume = bgmSlider.value;
        saveData.sfxVolume = sfxSlider.value;
        SaveManager.Instance.Save(saveData);
    }

    public override void LoadData()
    {
        SaveSettingData settingData = SaveManager.Instance.Load<SaveSettingData>();
        bgmSlider.value = settingData.bgmVolume;
        sfxSlider.value = settingData.sfxVolume;
        SetBGMVolume(settingData.bgmVolume);
        SetSFXVolume(settingData.sfxVolume);
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
