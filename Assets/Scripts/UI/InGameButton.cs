using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]

public abstract class InGameButton : MonoBehaviour
{
    private Button _button;
    protected Image Image { get; private set; }

    protected virtual void Awake()
    {
        Image = GetComponent<Image>();
        _button = GetComponent<Button>();

        _button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        if (TryClick())
        {
            InGameSfxManager.current.GoodClick();
        }
        else
        {
            InGameSfxManager.current.BadClick();
        }
    }

    protected abstract bool TryClick();

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(OnClick);
    }
    
    public virtual void CanClick(bool canClick)
    {
        Image.color = canClick ? Color.white : new Color(0.5f, 0.5f, 0.5f, 1f);
    }


    protected IEnumerator ExecuteAfterSound(Action action)
    {
        yield return 0;
        action.Invoke();
    }
}
