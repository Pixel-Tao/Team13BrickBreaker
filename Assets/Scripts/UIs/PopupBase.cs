using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupBase : MonoBehaviour
{
    public virtual void CloseButtonClick()
    {
        // 공통적으로 팝업을 닫는 함수
        UIManager.Instance.ClosePopup(this);
    }

    public virtual void LoadData()
    {
        // 팝업에 필요한 데이터를 불러오는 함수
        // 자식 클래스에서 필요시 override 하여 사용
    }

    public virtual void Init()
    {
        // 팝업 초기화 함수
        // 자식 클래스에서 필요시 override 하여 사용
    }
}
