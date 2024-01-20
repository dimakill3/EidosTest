using CharacterComponents;
using Factories;
using Services.SaveProgress;
using StateMachineComponents;
using UI.HUD;
using UnityEngine;
using EventProvider = Services.EventProviderComponents.EventProvider;

namespace Infrastructure.GameStates
{
    public class StartGameState : BaseState
    {
        private const string AimTargetTag = "Target";
        
        private readonly Canvas _canvas;
        private readonly Transform _spawnPoint;
        private readonly EventProvider _eventProvider;
        private readonly AllFactories _allFactories;
        private readonly SaveLoadService _saveLoadService;

        public StartGameState(StateMachine stateMachine, Canvas canvas, Transform spawnPoint,
            EventProvider eventProvider, AllFactories allFactories, SaveLoadService saveLoadService) : base(stateMachine)
        {
            _canvas = canvas;
            _spawnPoint = spawnPoint;
            _eventProvider = eventProvider;
            _allFactories = allFactories;
            _saveLoadService = saveLoadService;
        }

        public override void Enter()
        {
            SpawnPlayer();
            
            _stateMachine.EnterState<GameLoopState>();
        }

        private void SpawnPlayer()
        {
            CharacterView character = _allFactories.GetPlayerView(_spawnPoint.transform.position,
                _spawnPoint.transform.rotation, _spawnPoint.parent, _eventProvider);

            HudView hudView = _allFactories.GetHudView(_canvas.transform, _eventProvider, _saveLoadService);
            hudView.Enable();
            
            GameObject aimTarget = GameObject.FindWithTag(AimTargetTag);

            character.SetTarget(aimTarget.transform);
            character.Enable();
        }
    }
}