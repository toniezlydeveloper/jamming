using System;
using Core;
using Highscore;
using Internal.Runtime.Flow.States;
using Recipes;

namespace Flow.States
{
    public class GameplaySetupState : AState
    {
        public override void OnEnter()
        {
            IngredientsContainer.Apply();
            ResultsCounter.BrewedPotions.Clear();
            RecipesSaver.Clear();
        }

        public override Type OnUpdate() => typeof(GameplayState);
    }
}