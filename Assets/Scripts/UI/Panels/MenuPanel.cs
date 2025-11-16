using DG.Tweening;
using Flow.States;
using Highscore;
using Internal.Runtime.Flow.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Panels
{
    public class MenuPanel : AUIPanel
    {
        [Header("Navigation")]
        [SerializeField] private Button playButton;
        [SerializeField] private Button quitButton;
        
        [Header("Highscore")]
        [SerializeField] private GameObject highscoreHolder;
        [SerializeField] private TextMeshProUGUI highscoreContainer;
        [SerializeField] private string highscoreText;

        private void Start()
        {
            playButton.onClick.AddListener(RequestTransition<GameplaySetupState>);
            quitButton.onClick.AddListener(RequestTransition<QuitState>);
        }

        public override Tween Enable()
        {
            highscoreHolder.SetActive(ResultsHolder.TryGetHighScore(out int score));
            highscoreContainer.text = string.Format(highscoreText, score);
            return base.Enable();
        }
    }
}