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
        private readonly Rig _headAimRig;
        private readonly Rig _bodyAimRig;
        private readonly Rig _eyeAimRig;
        private readonly float _headRigAimWeight;
        private readonly float _bodyRigAimWeight;
        private readonly float _eyeRigAimWeight;
        private readonly MonoService _monoService;

        private Transform _target;

        public AimTargetState(StateMachine stateMachine, Transform headAimConstraint, Transform bodyAimConstraint,
            Transform eyeAimConstraint, Rig headAimRig, Rig bodyAimRig, Rig eyeAimRig, float headRigAimWeight,
            float bodyRigAimWeight, float eyeRigAimWeight, MonoService monoService)
            : base(stateMachine)
        {
            _headAimConstraint = headAimConstraint;
            _bodyAimConstraint = bodyAimConstraint;
            _eyeAimConstraint = eyeAimConstraint;
            _headAimRig = headAimRig;
            _bodyAimRig = bodyAimRig;
            _eyeAimRig = eyeAimRig;
            _headRigAimWeight = headRigAimWeight;
            _bodyRigAimWeight = bodyRigAimWeight;
            _eyeRigAimWeight = eyeRigAimWeight;
            _monoService = monoService;
        }

        public override void Enter(Transform target)
        {
            _target = target;
            
            _headAimRig.weight = _headRigAimWeight;
            _bodyAimRig.weight = _bodyRigAimWeight;
            _eyeAimRig.weight = _eyeRigAimWeight;

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