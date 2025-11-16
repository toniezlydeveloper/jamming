using System;
using Core;
using Flow.Setup;
using Highscore;
using Internal.Runtime.Dependencies.Core;
using Internal.Runtime.Flow.States;
using Recipes;
using UI.Elements;

namespace Flow.States
{
    public class GameplaySetupState : AState
    {
        private IHintPresenter _presenter = DependencyInjector.Get<IHintPresenter>();
        private GameSettings _settings;

        public GameplaySetupState(GameSettings settings)
        {
            _settings = settings;
        }

        public override void OnEnter()
        {
            _presenter.Present(new AddHintData
            {
                Text = "Pause",
                Code = _settings.PauseIcon
            });
            IngredientsContainer.Apply();
            ResultsCounter.BrewedPotions.Clear();
            RecipesSaver.Clear();
        }

        public override Type OnUpdate() => typeof(GameplayState);
    }
}