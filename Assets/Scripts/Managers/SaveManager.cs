using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private static SaveManager _instance;
    public static SaveManager Instance { get { Init(); return _instance; } }

    private static void Init()
    {
        if (_instance == null)
        {
            // GameManager 동적 생성
            GameObject go = new GameObject { name = "SaveManager" };
            _instance = go.AddComponent<SaveManager>();
            DontDestroyOnLoad(go);
        }
    }

    public void Save<T>(T data) where T : class
    {
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(typeof(T).Name, json);
    }

    public T Load<T>() where T : class
    {
        if (PlayerPrefs.HasKey(typeof(T).Name)) return null;

        string json = PlayerPrefs.GetString(typeof(T).Name);
        return JsonUtility.FromJson<T>(json);
    }
}
