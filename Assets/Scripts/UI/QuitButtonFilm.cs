using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class QuitButtonFilm : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool mouseIsOver;

    
    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseIsOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseIsOver = false;
    }

    public void OnClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
        Music.current.SelectSong(0);
    }
}
