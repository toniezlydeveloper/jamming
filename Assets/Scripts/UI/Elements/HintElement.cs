using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Elements
{
    public class HintElement : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textContainer;
        [SerializeField] private Image  iconContainer;

        public void Init(string text, Sprite icon)
        {
            textContainer.text = text;
            iconContainer.sprite = icon;
        }
    }
}