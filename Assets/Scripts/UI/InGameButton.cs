using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]

public abstract class InGameButton : MonoBehaviour
{
    private Button _button;
    private Image _image;

    protected virtual void Awake()
    {
        _image = GetComponent<Image>();
        _button = GetComponent<Button>();

        _button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        if (TestForSuccess())
        {
            InGameSfxManager.current.GoodClick();
        }
        else
        {
            InGameSfxManager.current.BadClick();
        }
    }

    protected abstract bool TestForSuccess();

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(OnClick);
    }
    
    public virtual void CanClick(bool canClick)
    {
        _image.color = canClick ? Color.white : new Color(0.5f, 0.5f, 0.5f, 1f);
    }


    protected IEnumerator ExecuteAfterSound(Action action)
    {
        yield return 0;
        action.Invoke();
    }
}
