using System;
using Flow.Setup;
using Highscore;
using Internal.Runtime.Dependencies.Core;
using Internal.Runtime.Flow.States;
using UI.Panels;

namespace Flow.States
{
    public class ResultsCheckState : AState
    {
        private IResultsCheckPresenter _presenter = DependencyInjector.Get<IResultsCheckPresenter>();
        private GameInput _input;

        public ResultsCheckState(GameInput input)
        {
            _input = input;
        }

        public override void OnEnter()
        {
            _presenter.Present(ResultsCounter.BrewedPotions.Count);
        }

        public override Type OnUpdate()
        {
            if (_input.PauseKey.action.triggered)
            {
                return typeof(GameplayState);
            }
            return base.OnUpdate();
        }
    }
}