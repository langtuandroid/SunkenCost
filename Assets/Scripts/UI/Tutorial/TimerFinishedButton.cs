using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerFinishedButton : MonoBehaviour
{
    private TextMeshProUGUI _text;
    private TutorialDimmerPanel _tutorialDimmerPanel;
    private GameObject _firstPopup;
    private GameObject _secondPopup;
    private GameObject _thirdPopup;

    private int _playerLives = 3;
    
    private void Awake()
    {
        _text = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        _firstPopup = transform.GetChild(0).gameObject;
        _secondPopup = transform.GetChild(1).gameObject;
        _thirdPopup = transform.GetChild(2).gameObject;

        _tutorialDimmerPanel = transform.parent.GetChild(0).GetComponent<TutorialDimmerPanel>();
    }

    private void Update()
    {
        if (PlayerController.current.Lives < _playerLives)
        {
            _playerLives--;

            if (_playerLives <= 0)
            {
                OutOfLives();
            }
        }
    }

    public void FirstTimerFinished()
    {
        _firstPopup.SetActive(true);
        _tutorialDimmerPanel.SetVisible(true);
        _text.text = "The first song has finished - since this is a folio presentation you can choose to spawn the Boss next turn, or keep playing (Boss arrives in "+ (16 - BattleManager.Current.Turn) +" turns). I'll give you free sticks to help out if you skip :)\n\n\n\n";
    }
    
    public void SecondTimerFinished()
    {
        _secondPopup.SetActive(true);
        _tutorialDimmerPanel.SetVisible(true);
    }

    private void OutOfLives()
    {
        _thirdPopup.SetActive(true);
        _tutorialDimmerPanel.SetVisible(true);
    }

    public void ClickedSkipToBoss()
    {
        InGameSfxManager.current.GoodClick();
        _tutorialDimmerPanel.SetVisible(false);
        _firstPopup.SetActive(false);
        BattleManager.Current.SkipToBoss();
    }

    public void ClickedKeepPlayingFirst()
    {
        InGameSfxManager.current.GoodClick();
        _tutorialDimmerPanel.SetVisible(false);
        _firstPopup.SetActive(false);
    }

    public void ClickedQuit()
    {
        InGameSfxManager.current.GoodClick();
        BattleManager.Current.Quit();
    }
    
    public void ClickedKeepPlayingSecond()
    {
        InGameSfxManager.current.GoodClick();
        _tutorialDimmerPanel.SetVisible(false);
        _secondPopup.SetActive(false);
    }

    public void ClickedKeepPlayingLives()
    {
        InGameSfxManager.current.GoodClick();
        _tutorialDimmerPanel.SetVisible(false);
        _thirdPopup.SetActive(false);
    }
}
