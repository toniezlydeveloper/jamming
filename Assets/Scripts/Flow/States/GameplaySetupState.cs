using System;
using Core;
using Internal.Runtime.Flow.States;

namespace Flow.States
{
    public class GameplaySetupState : AState
    {
        public override void OnEnter()
        {
            IngredientsContainer.Apply();
        }

        public override Type OnUpdate() => typeof(GameplayState);
    }
}