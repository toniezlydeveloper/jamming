using System;
using Flow.Setup;
using Internal.Runtime.Dependencies.Core;
using Internal.Runtime.Flow.States;
using Recipes;
using UI.Panels;

namespace Flow.States
{
    public class RecipesCheckState : AState
    {
        private IRecipesPresenter _presenter = DependencyInjector.Get<IRecipesPresenter>();
        private GameInput _input;

        public RecipesCheckState(GameInput input)
        {
            _input = input;
        }

        public override void OnEnter()
        {
            _presenter.Present(new RecipesInfo
            {
                Outputs = RecipesSaver.Read()
            });
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