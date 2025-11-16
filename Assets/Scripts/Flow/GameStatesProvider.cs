using Flow.Setup;
using Flow.States;
using Internal.Runtime.Dependencies.Core;
using Internal.Runtime.Flow.States;
using UnityEngine;

namespace Flow
{
    public class GameStatesProvider : AStatesProvider
    {
        [SerializeField] private GameReferences gameReferences;
        [SerializeField] private GameSettings gameSettings;
        [SerializeField] private GameInput gameInput;
    
        private void Awake()
        {
            AddState(new MenuState());
            AddState(new QuitState());
            AddState(new GameplaySetupState());
            AddState(new RecipesCheckState());
            AddState(new ResultsCheckState());
            AddState(new ResultsState());
            AddState(new GameplayState(gameSettings, gameInput, gameReferences));
            AddInitialState(new BootstrapState(gameSettings));
        }

        private void OnDestroy()
        {
            DependencyInjector.Clear();
        }
    }
}
