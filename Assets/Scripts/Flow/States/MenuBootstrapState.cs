using System;
using Flow.Setup;
using Internal.Runtime.Flow.States;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Flow.States
{
    public class MenuBootstrapState : AState
    {
        private AsyncOperation _loadingOperation;
        private GameSettings _gameSettings;

        public MenuBootstrapState(GameSettings gameSettings)
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

        private AsyncOperation StartLoadingGame() => SceneManager.LoadSceneAsync(_gameSettings.MenuName, LoadSceneMode.Single);

        private bool LoadedLevel() => _loadingOperation.isDone;

        private void Cache(AsyncOperation loadingOperation)
        {
            _loadingOperation = loadingOperation;
        }
    }
}