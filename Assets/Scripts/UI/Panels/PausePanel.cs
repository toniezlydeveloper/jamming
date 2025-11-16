using Flow.States;
using Internal.Runtime.Flow.UI;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Panels
{
    public class PausePanel : AUIPanel
    {
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button restartButton;
        [SerializeField] private Button quitButton;

        private void Start()
        {
            resumeButton.onClick.AddListener(RequestTransition<GameplayState>);
            restartButton.onClick.AddListener(RequestTransition<GameBootstrapState>);
            quitButton.onClick.AddListener(RequestTransition<QuitState>);
        }
    }
}