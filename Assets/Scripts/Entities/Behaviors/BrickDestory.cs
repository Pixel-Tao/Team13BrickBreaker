using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickDestory : MonoBehaviour
{
    private Brick brick;

    private void Awake()
    {
        brick = GetComponent<Brick>();
    }

    // Start is called before the first frame update
    void Start()
    {
        brick.OnBrickDestroyedEvent += DestoryBrick;
    }

    private void DestoryBrick(PlayerType playerType)
    {
        foreach (SpriteRenderer renderer in GetComponentsInChildren<SpriteRenderer>())
        {
            Color color = renderer.color;
            color.a = 0.3f;
            renderer.color = color;
        }

        foreach (Behaviour behaviour in GetComponentsInChildren<Behaviour>())
        {
            behaviour.enabled = false;
        }

        Destroy(gameObject, 1f);

        AudioManager.Instance.PlaySfx(AudioClipType.brick_break);
        GameManager.Instance.AddScrore(playerType, 100);
        GameManager.Instance.DropItem(transform.position);
        GameManager.Instance.DecreaseBrick();
    }
}
