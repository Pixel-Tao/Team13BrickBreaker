using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BoxContactType
{
    All,
    Top,
    Bottom,
    Left,
    Right
}

public class Box : MonoBehaviour
{
    // 박스 충돌 면
    public BoxContactType allowBoxContactType = BoxContactType.All;
    protected BoxCollider2D boxCollider;

    protected virtual void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public bool AllowBoxContact(Vector3 hitPosition)
    {
        if (allowBoxContactType == BoxContactType.All)
            return true;

        // BoxCollider2D의 면을 구분
        Vector2 colliderCenter = boxCollider.bounds.center;
        Vector2 colliderSize = boxCollider.size;

        // BoxCollider2D의 각 면 좌표 계산
        float left = colliderCenter.x - (colliderSize.x / 2);
        float right = colliderCenter.x + (colliderSize.x / 2);
        float top = colliderCenter.y + (colliderSize.y / 2);
        float bottom = colliderCenter.y - (colliderSize.y / 2);

        // hitPosition과 각 면의 거리를 비교하여 가장 가까운 면 구분

        if (allowBoxContactType == BoxContactType.Left)
        {
            return Mathf.Abs(hitPosition.x - left) < Mathf.Abs(hitPosition.x - right);
        }
        else if (allowBoxContactType == BoxContactType.Right)
        {
            return Mathf.Abs(hitPosition.x - right) < Mathf.Abs(hitPosition.x - left);
        }
        else if (allowBoxContactType == BoxContactType.Top)
        {
            return Mathf.Abs(hitPosition.y - top) < Mathf.Abs(hitPosition.y - bottom);
        }
        else if (allowBoxContactType == BoxContactType.Bottom)
        {
            return Mathf.Abs(hitPosition.y - bottom) < Mathf.Abs(hitPosition.y - top);
        }

        return false;
    }
}
