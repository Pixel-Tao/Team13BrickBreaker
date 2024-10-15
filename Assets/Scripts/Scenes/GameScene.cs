using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    void Start()
    {
        // Scene 진입점
        GameManager.Instance.GameStart();
    }
}