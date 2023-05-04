using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    public static MainManager Current;

    private void Awake()
    {
        Current = this;
        DontDestroyOnLoad(gameObject);
    }

    private void ChangeScene(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
 
        if (SceneManager.GetActiveScene().buildIndex != sceneNumber)
        {
            StartCoroutine("WaitForSceneLoad", sceneNumber);
        }
    }
 
    IEnumerator WaitForSceneLoad(int sceneNumber)
    {
        while (SceneManager.GetActiveScene().buildIndex != sceneNumber)
        {
            yield return null;
        }
 
        // Do anything after proper scene has been loaded
        if (SceneManager.GetActiveScene().buildIndex == sceneNumber)
        {
        }
    }

    public void LoadMenu()
    {
        ChangeScene(0);
    }

    public void LoadMap()
    {
        ChangeScene(1);
    }

    public void LoadOfferScreen()
    {
        ChangeScene(2);
    }

    public void LoadNextBattle()
    {
        ChangeScene(3);
    }

    public void StartNewRun()
    {
        RunProgress.Initialise();
        LoadOfferScreen();
    }
}
