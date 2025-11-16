using System;
using Flow.Setup;
using Internal.Runtime.Flow.States;

namespace Flow.States
{
    public class PauseState : AState
    {
        private GameInput _input;

        public PauseState(GameInput input)
        {
            _input = input;
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