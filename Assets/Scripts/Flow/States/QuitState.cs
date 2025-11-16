using Internal.Runtime.Flow.States;

namespace Flow.States
{
    public class QuitState : AState
    {
        public override void OnEnter()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
#else
            UnityEngine.Application.Quit();
#endif
        }
    }
}