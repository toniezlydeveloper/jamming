using Audio;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Elements
{
    public class ButtonWrapper : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(() => FindAnyObjectByType<SfxPlayer>().Play(SfxType.UIClick));
        }
    }
}