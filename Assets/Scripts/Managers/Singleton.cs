using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    public static T Instance { get { Init(); return _instance; } }

    private static void Init()
    {
        if (_instance == null)
        {
            string name = typeof(T).Name;
            // GameManager 동적 생성
            GameObject go = new GameObject { name = name };
            _instance = go.AddComponent<T>();
            DontDestroyOnLoad(go);
        }
    }
}
