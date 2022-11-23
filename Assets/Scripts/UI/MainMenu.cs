using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public static void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public static void PlayFilm()
    {
        Music.current.SelectSong(3);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }
}
