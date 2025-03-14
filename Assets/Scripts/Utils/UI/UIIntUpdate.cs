using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIIntUpdate : MonoBehaviour
{
    //public IntData intData;
    public TextMeshProUGUI uiTextValue;

    void Start()
    {
        UpdateValueOnUi();
    }

    public void UpdateValueOnUi()
    {
        //uiTextValue.text = intData.value.ToString();
    }
}
