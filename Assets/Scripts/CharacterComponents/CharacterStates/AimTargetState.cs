using _Assets.Scripts.Core.StateMachine;
using Services;
using StateMachineComponents;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace CharacterComponents.CharacterStates
{
    public class AimTargetState : BasePayloadedState<Transform>
    {
        private readonly Transform _headAimConstraint;
        private readonly Transform _bodyAimConstraint;
        private readonly Transform _eyeAimConstraint;
        private readonly Rig _headRig;
        private readonly Rig _bodyRig;
        private readonly Rig _eyeRig;
        private readonly float _headRigAimWeight;
        private readonly float _bodyRigAimWeight;
        private readonly float _eyeRigAimWeight;
        private readonly MonoService _monoService;

        private Transform _target;

        public AimTargetState(StateMachine stateMachine, Transform headAimConstraint, Transform bodyAimConstraint,
            Transform eyeAimConstraint, Rig headRig, Rig bodyRig, Rig eyeRig, float headRigAimWeight,
            float bodyRigAimWeight, float eyeRigAimWeight, MonoService monoService)
            : base(stateMachine)
        {
            _headAimConstraint = headAimConstraint;
            _bodyAimConstraint = bodyAimConstraint;
            _eyeAimConstraint = eyeAimConstraint;
            _headRig = headRig;
            _bodyRig = bodyRig;
            _eyeRig = eyeRig;
            _headRigAimWeight = headRigAimWeight;
            _bodyRigAimWeight = bodyRigAimWeight;
            _eyeRigAimWeight = eyeRigAimWeight;
            _monoService = monoService;
        }

        public override void Enter(Transform target)
        {
            _target = target;
            
            _headRig.weight = _headRigAimWeight;
            _bodyRig.weight = _bodyRigAimWeight;
            _eyeRig.weight = _eyeRigAimWeight;

            _monoService.UpdateTick += OnTickUpdate;
        }

        public override void Exit() => 
            _monoService.UpdateTick -= OnTickUpdate;

        private void OnTickUpdate()
        {
            var position = _target.position;
            _headAimConstraint.position = position;
            _bodyAimConstraint.position = position;
            _eyeAimConstraint.position = position;
        }
    }
}