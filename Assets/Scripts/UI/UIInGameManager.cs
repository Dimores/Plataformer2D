using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Orby.Core.Singleton;

public class UIInGameManager : Singleton<UIInGameManager>
{
    public static void UpdateTextOnUI(TextMeshProUGUI text, string value)
    {
        text.text = value;
    }
}
