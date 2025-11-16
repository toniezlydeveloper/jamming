using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Elements
{
    public class IngredientElement : MonoBehaviour
    {
        [SerializeField] private GameObject holder;
        [SerializeField] private Button button;
        [SerializeField] private Image iconContainer;
        [SerializeField] private TextMeshProUGUI textContainer;
        [SerializeField] private TextMeshProUGUI countContainer;

        public TextMeshProUGUI CountContainer => countContainer;
        public GameObject Holder => holder;
        public Button Button => button;
        public Image IconContainer => iconContainer;
        public TextMeshProUGUI TextContainer => textContainer;

        private void OnValidate()
        {
            button = GetComponentInChildren<Button>();
        }
    }
}