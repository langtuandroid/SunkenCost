using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Cost : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textMeshProUGUI;

    public void UpdateCost(int cost)
    {
        _textMeshProUGUI.text = cost.ToString();
    }
}
