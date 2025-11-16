using Flow.States;
using Internal.Runtime.Dependencies.Core;
using Internal.Runtime.Flow.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Panels
{
    public interface IResultsPresenter : IDependency
    {
        void Present(ResultsInfo info);
    }
    
    public class ResultsInfo
    {
        public int Score { get; set; }
        public int Highscore { get; set; }
        public bool GotHighscore { get; set; }
    }

    public class ResultsPanel : AUIPanel, IResultsPresenter
    {
        [SerializeField] private GameObject highscoreHolder;
        [SerializeField] private GameObject scoreHolder;

        [SerializeField] private string scoreT;
        
        [SerializeField] private TextMeshProUGUI[] scoreContainers;

        [SerializeField] private Button restartButton;
        [SerializeField] private Button quitButton;


        private void Start()
        {
            restartButton.onClick.AddListener(RequestTransition<MenuBootstrapState>);
            quitButton.onClick.AddListener(RequestTransition<QuitState>);
        }

        public void Present(ResultsInfo info)
        {
            highscoreHolder.SetActive(info.GotHighscore);
            scoreHolder.SetActive(!info.GotHighscore);
            
            foreach (TextMeshProUGUI s in scoreContainers)
            {
                s.text = string.Format(scoreT, info.Score);
            }
        }
    }
}