using _Assets.Scripts.Core.StateMachine;
using Services;
using StateMachineComponents;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace CharacterComponents.CharacterStates
{
    public class FreeState : BasePayloadedState<Transform>
    {
        private const float MinHeadUpdateCooldown = 5;
        private const float MinEyeUpdateCooldown = 5;
        private const float HeadUpdateChance = 0.01f;
        private const float EyeUpdateChance = 0.01f;
        private const float AimAtTargetChance = 0.3f;

        private readonly Transform _headAimConstraint;
        private readonly Transform _eyeAimConstraint;
        private readonly Rig _headRig;
        private readonly Rig _eyeRig;
        private readonly MonoService _monoService;

        private float _currentTime;
        private float _lastHeadUpdate;
        private float _lastEyeUpdate;
        private float normalizedRandom => Random.Range(0, 1f);

        private Transform _target;
        private Transform _currentHeadTarget;
        private Transform _currentEyeTarget;

        public FreeState(StateMachine stateMachine, Transform headAimConstraint, Transform eyeAimConstraint, Rig headRig,
            Rig eyeRig, MonoService monoService) : base(stateMachine)
        {
            _headAimConstraint = headAimConstraint;
            _eyeAimConstraint = eyeAimConstraint;
            _headRig = headRig;
            _eyeRig = eyeRig;

            _monoService = monoService;
        }

        public override void Enter(Transform target)
        {
            _target = target;

            _currentTime = 0;

            UpdateHead(true);
            UpdateEye(true);
            
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

            if (_currentTime - _lastHeadUpdate > MinHeadUpdateCooldown && normalizedRandom < HeadUpdateChance)
                UpdateHead();

            _headAimConstraint.position = _currentHeadTarget.position;
            
            if (_currentTime - _lastEyeUpdate > MinEyeUpdateCooldown && normalizedRandom < EyeUpdateChance)
                UpdateEye();
            
            _eyeAimConstraint.position = _currentEyeTarget.position;
        }

        private void UpdateHead(bool isForce = false)
        {
            Transform newTarget = GetSomeTransform(isForce);

            if (newTarget != _target)
                Object.Destroy(_currentHeadTarget.gameObject);

            _currentHeadTarget = newTarget;

            _headRig.weight = normalizedRandom;

            _lastHeadUpdate = _currentTime;
        }

        private void UpdateEye(bool isForce = false)
        {
            Transform newTarget = GetSomeTransform(isForce);

            if (newTarget != _target)
                Object.Destroy(_currentEyeTarget.gameObject);

            _currentEyeTarget = newTarget;
            
            _eyeRig.weight = normalizedRandom;
            
            _lastEyeUpdate = _currentTime;
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