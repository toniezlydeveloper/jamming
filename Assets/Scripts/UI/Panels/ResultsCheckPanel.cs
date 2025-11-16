using Flow.States;
using Internal.Runtime.Dependencies.Core;
using Internal.Runtime.Flow.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Panels
{
    public interface IResultsCheckPresenter : IDependency
    {
        void Present(int potionCount);
    }
    
    public class ResultsCheckPanel : AUIPanel, IResultsCheckPresenter
    {
        [SerializeField] private Button finishButton;
        [SerializeField] private Button backButton;
        [SerializeField] private TextMeshProUGUI container;
        [SerializeField] private string text;

        private void Start()
        {
            finishButton.onClick.AddListener(RequestTransition<ResultsState>);
            backButton.onClick.AddListener(RequestTransition<GameplayState>);
        }

        public void Present(int potionCount)
        {
            container.text = string.Format(text, potionCount);
        }
    }
}