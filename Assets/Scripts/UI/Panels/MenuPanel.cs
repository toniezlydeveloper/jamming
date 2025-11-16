using Flow.States;
using Internal.Runtime.Flow.UI;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Panels
{
    public class MenuPanel : AUIPanel
    {
        [SerializeField] private Button playButton;
        [SerializeField] private Button quitButton;

        private void Start()
        {
            playButton.onClick.AddListener(RequestTransition<GameplaySetupState>);
            quitButton.onClick.AddListener(RequestTransition<QuitState>);
        }
    }
}