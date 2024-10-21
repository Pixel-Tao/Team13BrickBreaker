using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    // 챌린지 2주차 월요일 세션 내용 정복
    // MonoBehaviour 를 상속받은 T 클래스의 인스턴스
    private static T _instance;
    // get 에서 Init 함수를 실행하고 _instance 를 반환
    public static T Instance { get { Init(); return _instance; } }
    // Init 함수에서 _instance 가 null 일 때 생성
    private static void Init()
    {
        if (_instance == null)
        {
            // typeof(T) 는 T 클래스의 Type 을 반환
            string name = typeof(T).Name;
            // GameObject 를 생성하고 T 클래스를 추가
            GameObject go = new GameObject { name = name };
            // T 클래스를 추가하고 _instance 에 할당
            _instance = go.AddComponent<T>();
            // 씬이 변경되어도 삭제되지 않도록 설정
            DontDestroyOnLoad(go);
        }
    }
}
