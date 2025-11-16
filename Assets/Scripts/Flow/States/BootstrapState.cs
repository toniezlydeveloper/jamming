using System;
using Flow.Setup;
using Internal.Runtime.Flow.States;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Flow.States
{
    public class BootstrapState : AState
    {
        private AsyncOperation _loadingOperation;
        private GameSettings _gameSettings;

        public BootstrapState(GameSettings gameSettings)
        {
            _gameSettings = gameSettings;
        }

        public override void OnEnter()
        {
            Cache(StartLoadingGame());
        }

        public override Type OnUpdate()
        {
            if (LoadedLevel())
            {
                return typeof(MenuState);
            }

            return null;
        }

        private AsyncOperation StartLoadingGame() => SceneManager.LoadSceneAsync(_gameSettings.LevelName, LoadSceneMode.Single);

        private bool LoadedLevel() => _loadingOperation.isDone;

        private void Cache(AsyncOperation loadingOperation)
        {
            _loadingOperation = loadingOperation;
        }
    }
}