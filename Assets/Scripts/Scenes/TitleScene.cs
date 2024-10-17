using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScene : MonoBehaviour
{
    [SerializeField] private GameObject titleUIPrefab;

    private TitleUI titleUI;

    private void Start()
    {
        // TitleScene 진입점.
        titleUI = Instantiate(titleUIPrefab, transform).GetComponent<TitleUI>();
        //GameManager.Instance.TitleStart();
    }
}
