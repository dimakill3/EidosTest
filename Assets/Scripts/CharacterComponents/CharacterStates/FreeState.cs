using _Assets.Scripts.Core.StateMachine;
using Services;
using StateMachineComponents;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace CharacterComponents.CharacterStates
{
    public class FreeState : BasePayloadedState<Transform>
    {
        private const float UpdateCooldown = 5;
        private const float UpdateChance = 0.01f;
        private const float AimAtTargetChance = 0.3f;

        private readonly Transform _headAimConstraint;
        private readonly Transform _bodyAimConstraint;
        private readonly Transform _eyeAimConstraint;
        private readonly Rig _headAimRig;
        private readonly Rig _bodyAimRig;
        private readonly Rig _eyeAimRig;
        private readonly MonoService _monoService;

        private float _currentTime;
        private float _lastHeadUpdate;
        private float _lastBodyUpdate;
        private float _lastEyeUpdate;
        private float normalizedRandom => Random.Range(0, 1f);

        private Transform _target;
        private Transform _currentHeadTarget;
        private Transform _currentBodyTarget;
        private Transform _currentEyeTarget;

        public FreeState(StateMachine stateMachine, Transform headAimConstraint, Transform bodyAimConstraint,
            Transform eyeAimConstraint, Rig headAimRig, Rig bodyAimRig, Rig eyeAimRig, MonoService monoService) : base(stateMachine)
        {
            _headAimConstraint = headAimConstraint;
            _bodyAimConstraint = bodyAimConstraint;
            _eyeAimConstraint = eyeAimConstraint;
            _headAimRig = headAimRig;
            _bodyAimRig = bodyAimRig;
            _eyeAimRig = eyeAimRig;

            _monoService = monoService;
        }

        public override void Enter(Transform target)
        {
            _target = target;
            _currentHeadTarget = _target;
            _currentBodyTarget = _target;
            _currentEyeTarget = _target;

            _currentTime = 0;

            UpdateConstraintAimTarget(ref _currentHeadTarget, _headAimRig, out _lastHeadUpdate, true);
            UpdateConstraintAimTarget(ref _currentBodyTarget, _bodyAimRig, out _lastBodyUpdate, true);
            UpdateConstraintAimTarget(ref _currentEyeTarget, _eyeAimRig, out _lastEyeUpdate, true);

            _monoService.UpdateTick += OnUpdateTick;
        }

        public override void Exit()
        {
            _monoService.UpdateTick -= OnUpdateTick;
            
            if (_currentEyeTarget != _target)
                Object.Destroy(_currentEyeTarget.gameObject);
            
            if (_currentHeadTarget != _target)
                Object.Destroy(_currentEyeTarget.gameObject);
        }

        private void OnUpdateTick()
        {
            _currentTime += Time.deltaTime;

            if (NeedUpdate(_lastHeadUpdate))
                UpdateConstraintAimTarget(ref _currentHeadTarget, _headAimRig, out _lastHeadUpdate);

            _headAimConstraint.position = _currentHeadTarget.position;
            
            if (NeedUpdate(_lastBodyUpdate))
                UpdateConstraintAimTarget(ref _currentBodyTarget, _bodyAimRig, out _lastBodyUpdate);
            
            _bodyAimConstraint.position = _currentBodyTarget.position;
            
            if (NeedUpdate(_lastEyeUpdate))
                UpdateConstraintAimTarget(ref _currentEyeTarget, _eyeAimRig, out _lastEyeUpdate);
            
            _eyeAimConstraint.position = _currentEyeTarget.position;
        }

        private bool NeedUpdate(float lastUpdateTime) => 
            _currentTime - lastUpdateTime > UpdateCooldown && normalizedRandom < UpdateChance;

        private void UpdateConstraintAimTarget(ref Transform currentTarget, Rig aimRig, out float lastUpdateTime,
            bool isForce = false)
        {
            Transform newTarget = GetSomeTransform(isForce);

            if (currentTarget != _target)
                Object.Destroy(currentTarget.gameObject);

            currentTarget = newTarget;

            aimRig.weight = normalizedRandom;

            lastUpdateTime = _currentTime;
        }

        private Transform GetSomeTransform(bool isForce)
        {
            Transform targetTransform;
                
            if (!isForce && normalizedRandom < AimAtTargetChance)
                targetTransform = _target;
            else
            {
                GameObject empty = new GameObject();
                empty.transform.position = new Vector3(Random.Range(0, 3), Random.Range(0, 4), Random.Range(-3, 3));

                targetTransform = empty.transform;
            }
            
            return targetTransform;
        }
    }
}