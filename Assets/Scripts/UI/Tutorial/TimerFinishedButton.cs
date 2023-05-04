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
        if (Player.Current.Health < _playerLives)
        {
            _playerLives--;

            if (_playerLives <= 0)
            {
                OutOfLives();
            }
        }
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
    }

    public void ClickedKeepPlayingFirst()
    {
        InGameSfxManager.current.GoodClick();
        _tutorialDimmerPanel.SetVisible(false);
        _firstPopup.SetActive(false);
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
