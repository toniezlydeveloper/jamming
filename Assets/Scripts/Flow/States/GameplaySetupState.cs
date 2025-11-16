using System;
using Core;
using Highscore;
using Internal.Runtime.Flow.States;

namespace Flow.States
{
    public class GameplaySetupState : AState
    {
        public override void OnEnter()
        {
            IngredientsContainer.Apply();
            ResultsCounter.BrewedPotions.Clear();
        }

        public override Type OnUpdate() => typeof(GameplayState);
    }
}