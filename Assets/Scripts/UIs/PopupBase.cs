using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupBase : MonoBehaviour
{
    public virtual void CloseButtonClick()
    {
        UIManager.Instance.ClosePopup(this);
    }

    public virtual void LoadData()
    {

    }
}
