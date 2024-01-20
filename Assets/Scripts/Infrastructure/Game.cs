using Factories;
using Infrastructure.GameStates;
using Services.SaveProgress;
using StateMachineComponents;
using UnityEngine;
using EventProvider = Services.EventProviderComponents.EventProvider;

namespace Infrastructure
{
    public class Game
    {
        private readonly EventProvider _eventProvider;
        private readonly AllFactories _allFactories;
        private readonly SaveLoadService _saveLoadService;
        private StateMachine _stateMachine;

        public Game(Canvas canvas, Transform spawnPoint, EventProvider eventProvider, AllFactories allFactories,
            SaveLoadService saveLoadService)
        {
            _eventProvider = eventProvider;
            _allFactories = allFactories;
            _saveLoadService = saveLoadService;

            _stateMachine = new StateMachine();
            _stateMachine.AddState(new StartGameState(_stateMachine, canvas, spawnPoint, _eventProvider, _allFactories,
                _saveLoadService));
            _stateMachine.AddState(new GameLoopState(_stateMachine));
        }

        public void Activate() => 
            _stateMachine.EnterState<StartGameState>();
    }
}