using TMPro;
using Orby.Core.Singleton;

namespace Orby.UI
{
    public class UIInGameManager : Singleton<UIInGameManager>
    {
        public static void UpdateTextOnUI(TextMeshProUGUI text, string value)
        {
            text.text = value;
        }
    }
}
