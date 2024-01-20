using CharacterComponents.CharacterStates;
using Enums;
using Events;
using SaveData;
using Services;
using Services.SaveProgress;
using StateMachineComponents;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using EventProvider = Services.EventProviderComponents.EventProvider;

namespace CharacterComponents
{
    [RequireComponent(typeof(Animator))]
    public class CharacterView : MonoBehaviour, ISavedProgress
    {
        [Header("Main")]
        [SerializeField] private AimModeEnum defaultAimMode;
        [Space]
        [Header("AimConstraints")]
        [SerializeField] private Transform headAimConstraint;
        [SerializeField] private Transform bodyAimConstraint;
        [SerializeField] private Transform eyeAimConstraint;
        [SerializeField] private Rig headAimRig;
        [SerializeField] private Rig bodyAimRig;
        [SerializeField] private Rig eyeAimRig;
        [Range(0, 1)]
        [SerializeField] private float headAimRigWeight;
        [Range(0, 1)]
        [SerializeField] private float bodyAimRigWeight;
        [Range(0, 1)]
        [SerializeField] private float eyeAimRigWeight;
        
        private AimModeEnum _currentAimMode = AimModeEnum.None;
        private Transform _target;
        private StateMachine _stateMachine;
        private EventProvider _eventProvider;
        private MonoService _monoService;

        public void Initialize(EventProvider eventProvider, MonoService monoService)
        {
            _eventProvider = eventProvider;
            _monoService = monoService;
            
            _stateMachine = new StateMachine();
            _stateMachine.AddState(new AimTargetState(_stateMachine, headAimConstraint, bodyAimConstraint,
                eyeAimConstraint, headAimRig, bodyAimRig, eyeAimRig,
                headAimRigWeight, bodyAimRigWeight, eyeAimRigWeight,
                _monoService));
            _stateMachine.AddState(new FreeState(_stateMachine, headAimConstraint, eyeAimConstraint,
                headAimRig, eyeAimRig, _monoService));
            _stateMachine.AddState(new AvoidTargetState(_stateMachine, headAimConstraint));
        }

        public void Enable()
        {
            ChangeAimMode(new ChangeAimModeEvent(defaultAimMode));
            
            _eventProvider.Subscribe<ChangeAimModeEvent>(ChangeAimMode);
        }

        public void Disable()
        {
            _stateMachine.Disable();

            ChangeAimMode(new ChangeAimModeEvent(AimModeEnum.None));
            
            _eventProvider.UnSubscribe<ChangeAimModeEvent>(ChangeAimMode);
        }

        public void LoadProgress(GameSaveData gameSaveData)
        {
            ChangeAimMode(new ChangeAimModeEvent(gameSaveData.AimModeEnum));
            _target.position = gameSaveData.AimPosition.ToUnityVector3();
        }

        public void UpdateProgress(GameSaveData gameSaveData)
        {
            gameSaveData.AimModeEnum = _currentAimMode;
            gameSaveData.AimPosition = _target.position.ToVector3Data();
        }

        public void SetTarget(Transform target) => 
            _target = target;

        private void ChangeAimMode(ChangeAimModeEvent changeAimModeEvent)
        {
            if (_currentAimMode == changeAimModeEvent.AimModeEnum)
                return;
            
            _currentAimMode = changeAimModeEvent.AimModeEnum;

            switch (_currentAimMode)
            {
                case AimModeEnum.None:
                    _stateMachine.Disable();
                    break;
                case AimModeEnum.Aim:
                    _stateMachine.EnterState<AimTargetState, Transform>(_target);
                    break;
                case AimModeEnum.Free:
                    _stateMachine.EnterState<FreeState, Transform>(_target);
                    break;
                case AimModeEnum.Avoid:
                    _stateMachine.EnterState<AvoidTargetState>();
                    break;
            }
            
            _eventProvider.Invoke(new AimModeChangedEvent(_currentAimMode));
        }
    }
}