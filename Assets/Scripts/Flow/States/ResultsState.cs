using Highscore;
using Internal.Runtime.Dependencies.Core;
using Internal.Runtime.Flow.States;
using UI.Panels;

namespace Flow.States
{
    public class ResultsState : AState
    {
        private IResultsPresenter _presenter = DependencyInjector.Get<IResultsPresenter>();
        
        public override void OnEnter()
        {
            bool isHighscore = ResultsCounter.TryGetHighScore(out int score, out int highscore);
            
            _presenter.Present(new ResultsInfo
            {
                Score = score,
                Highscore = highscore,
                GotHighscore = isHighscore
            });
        }
    }
}