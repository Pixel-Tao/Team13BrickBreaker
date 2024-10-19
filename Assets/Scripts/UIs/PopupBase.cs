using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupBase : MonoBehaviour
{
    public void CloseButtonClick()
    {
        UIManager.Instance.ClosePopup(this);
    }
}
