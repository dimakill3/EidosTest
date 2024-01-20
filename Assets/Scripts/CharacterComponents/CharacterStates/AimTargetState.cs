using StateMachineComponents;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace CharacterComponents.CharacterStates
{
    public class AimTargetState : BaseState
    {
        private readonly Transform _aimTargetContainer;
        private readonly Rig _headRig;
        private readonly Rig _bodyRig;
        private readonly Rig _eyeRig;
        private readonly float _headRigAimWeight;
        private readonly float _bodyRigAimWeight;
        private readonly float _eyeRigAimWeight;

        public AimTargetState(StateMachine stateMachine, Transform aimTargetContainer, Rig headRig, Rig bodyRig,
            Rig eyeRig, float headRigAimWeight, float bodyRigAimWeight, float eyeRigAimWeight) : base(stateMachine)
        {
            _aimTargetContainer = aimTargetContainer;
            _headRig = headRig;
            _bodyRig = bodyRig;
            _eyeRig = eyeRig;
            _headRigAimWeight = headRigAimWeight;
            _bodyRigAimWeight = bodyRigAimWeight;
            _eyeRigAimWeight = eyeRigAimWeight;
        }

        public override void Enter()
        {
            _headRig.weight = _headRigAimWeight;
            _bodyRig.weight = _bodyRigAimWeight;
            _eyeRig.weight = _eyeRigAimWeight;
        }
    }
}