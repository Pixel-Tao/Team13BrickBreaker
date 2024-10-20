using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveSettingData 
{
    public float bgmVolume = 0.5f;
    public float sfxVolume = 0.5f;
    public AudioClipType titleBGM = AudioClipType.bgm1;
    public AudioClipType gameBGM = AudioClipType.bgm2;
}
