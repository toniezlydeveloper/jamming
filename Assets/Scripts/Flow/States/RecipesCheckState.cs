using Internal.Runtime.Dependencies.Core;
using Internal.Runtime.Flow.States;
using Recipes;
using UI.Panels;

namespace Flow.States
{
    public class RecipesCheckState : AState
    {
        private IRecipesPresenter _presenter = DependencyInjector.Get<IRecipesPresenter>();
        
        public override void OnEnter()
        {
            _presenter.Present(new RecipesInfo
            {
                Outputs = RecipesSaver.Read()
            });
        }
    }
}