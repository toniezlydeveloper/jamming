using Highscore;
using Internal.Runtime.Dependencies.Core;
using Internal.Runtime.Flow.States;
using UI.Panels;

namespace Flow.States
{
    public class ResultsCheckState : AState
    {
        private IResultsCheckPresenter _presenter = DependencyInjector.Get<IResultsCheckPresenter>();
        
        public override void OnEnter()
        {
            _presenter.Present(ResultsCounter.BrewedPotions.Count);
        }
    }
}