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
        [SerializeField] private AimModeEnum defaultAimMode;
        [SerializeField] private Animator animator;
        [SerializeField] private Transform aimTargetContainer;
        [SerializeField] private Rig headRig;
        [SerializeField] private Rig bodyRig;
        [SerializeField] private Rig eyeRig;
        [SerializeField] private MultiAimConstraint headAimConstraint;
        [SerializeField] private MultiAimConstraint leftEyeAimConstraint;
        [SerializeField] private MultiAimConstraint rightEyeAimConstraint;
        [SerializeField] private RigBuilder rigBuilder;
        [Range(0, 1)]
        [SerializeField] private float headRigAimWeight;
        [Range(0, 1)]
        [SerializeField] private float bodyRigAimWeight;
        [Range(0, 1)]
        [SerializeField] private float eyeRigAimWeight;
        
        private AimModeEnum _currentAimMode = AimModeEnum.None;
        private StateMachine _stateMachine;
        private EventProvider _eventProvider;
        private MonoService _monoService;

        public void Initialize(EventProvider eventProvider, MonoService monoService)
        {
            _eventProvider = eventProvider;
            _monoService = monoService;
            
            _stateMachine = new StateMachine();
            _stateMachine.AddState(new AimTargetState(_stateMachine, aimTargetContainer, headRig, bodyRig, eyeRig,
                headRigAimWeight, bodyRigAimWeight, eyeRigAimWeight));
            _stateMachine.AddState(new FreeState(_stateMachine, aimTargetContainer, headRig, bodyRig, eyeRig,
                headAimConstraint, leftEyeAimConstraint, rightEyeAimConstraint, rigBuilder, _monoService));
            _stateMachine.AddState(new AvoidTargetState(_stateMachine, aimTargetContainer));
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
                    _stateMachine.EnterState<AimTargetState>();
                    break;
                case AimModeEnum.Free:
                    _stateMachine.EnterState<FreeState>();
                    break;
                case AimModeEnum.Avoid:
                    _stateMachine.EnterState<AvoidTargetState>();
                    break;
            }
            
            _eventProvider.Invoke(new AimModeChangedEvent(_currentAimMode));
        }

        public void LoadProgress(GameSaveData gameSaveData)
        {
            ChangeAimMode(new ChangeAimModeEvent(gameSaveData.AimModeEnum));
            aimTargetContainer.position = gameSaveData.AimPosition.ToUnityVector3();
        }

        public void UpdateProgress(GameSaveData gameSaveData)
        {
            gameSaveData.AimModeEnum = _currentAimMode;
            gameSaveData.AimPosition = aimTargetContainer.position.ToVector3Data();
        }

        [ContextMenu("Inactive")]
        public void SetInactive()
        {
            foreach (RigLayer rigLayer in rigBuilder.layers)
            {
                rigLayer.active = false;
            }
        }
        
        [ContextMenu("BuildRig")]
        public void BuildRig()
        {
            rigBuilder.Build();
        }
        
        [ContextMenu("Active")]
        public void SetActive()
        {
            foreach (RigLayer rigLayer in rigBuilder.layers)
            {
                rigLayer.active = true;
            }
        }
    }
}