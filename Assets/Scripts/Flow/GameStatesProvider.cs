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
            AddState(new GameplaySetupState(gameSettings));
            AddState(new RecipesCheckState(gameInput));
            AddState(new ResultsCheckState(gameInput));
            AddState(new PauseState(gameInput));
            AddState(new ResultsState());
            AddState(new GameplayState(gameSettings, gameInput, gameReferences));
            AddState(new GameBootstrapState(gameSettings));
            AddInitialState(new MenuBootstrapState(gameSettings));
        }

        private void OnDestroy()
        {
            DependencyInjector.Clear();
        }

        [ContextMenu(nameof(Clear))]
        private void Clear()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}
