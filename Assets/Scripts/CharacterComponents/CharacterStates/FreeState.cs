using Services;
using StateMachineComponents;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace CharacterComponents.CharacterStates
{
    public class FreeState : BaseState
    {
        private const float MinHeadUpdateCooldown = 5;
        private const float MinEyeUpdateCooldown = 5;
        private const float HeadUpdateChance = 0.01f;
        private const float EyeUpdateChance = 0.01f;
        private const float AimAtTargetChance = 0.3f;
        
        private readonly Transform _aimTargetContainer;
        private readonly Rig _headRig;
        private readonly Rig _bodyRig;
        private readonly Rig _eyeRig;
        private readonly MultiAimConstraint _headAimConstraint;
        private readonly MultiAimConstraint _leftEyeAimConstraint;
        private readonly MultiAimConstraint _rightEyeAimConstraint;
        private readonly RigBuilder _rigBuilder;
        private readonly MonoService _monoService;

        private float _currentTime;
        private float _lastHeadUpdate;
        private float _lastEyeUpdate;
        private float randomChance => Random.Range(0, 1f);

        public FreeState(StateMachine stateMachine, Transform aimTargetContainer, Rig headRig, Rig bodyRig, Rig eyeRig,
            MultiAimConstraint headAimConstraint, MultiAimConstraint leftEyeAimConstraint,
            MultiAimConstraint rightEyeAimConstraint, RigBuilder rigBuilder, MonoService monoService)
            : base(stateMachine)
        {
            _aimTargetContainer = aimTargetContainer;
            _headRig = headRig;
            _bodyRig = bodyRig;
            _eyeRig = eyeRig;
            
            _headAimConstraint = headAimConstraint;
            _leftEyeAimConstraint = leftEyeAimConstraint;
            _rightEyeAimConstraint = rightEyeAimConstraint;
            _rigBuilder = rigBuilder;

            _monoService = monoService;
        }

        public override void Enter()
        {
            _monoService.UpdateTick += OnUpdateTick;
            
            _currentTime = 0;
            
            UpdateHead(true);
            //UpdateEye(true);
        }

        public override void Exit()
        {
            _monoService.UpdateTick -= OnUpdateTick;
            
            var weightedTransform = new WeightedTransform(_aimTargetContainer, 1);
            ClearOldAndSetNewAimTarget(_headAimConstraint, weightedTransform);
            ClearOldAndSetNewAimTarget(_leftEyeAimConstraint, weightedTransform);
            ClearOldAndSetNewAimTarget(_rightEyeAimConstraint, weightedTransform);
        }

        private void OnUpdateTick()
        {
            _currentTime += Time.deltaTime;

            if (_currentTime - _lastHeadUpdate > MinHeadUpdateCooldown && randomChance < HeadUpdateChance)
                UpdateHead();

            /*if (_currentTime - _lastEyeUpdate > MinEyeUpdateCooldown && randomChance < EyeUpdateChance)
                UpdateEye();*/
        }

        private void UpdateHead(bool isForce = false)
        {
            WeightedTransform weightedTransform = GetWeightedTransform(isForce);
            
            ClearOldAndSetNewAimTarget(_headAimConstraint, weightedTransform);
            _headRig.weight = Random.Range(0.3f, 0.5f);

            _lastHeadUpdate = _currentTime;
        }

        private void UpdateEye(bool isForce = false)
        {
            WeightedTransform weightedTransform = GetWeightedTransform(isForce);

            ClearOldAndSetNewAimTarget(_leftEyeAimConstraint, weightedTransform);
            ClearOldAndSetNewAimTarget(_rightEyeAimConstraint, weightedTransform);
            _eyeRig.weight = Random.Range(0.3f, 0.5f);
            
            _lastEyeUpdate = _currentTime;
        }

        private WeightedTransform GetWeightedTransform(bool isForce)
        {
            Transform targetTransform;
                
            if (!isForce && randomChance < AimAtTargetChance)
                targetTransform = _aimTargetContainer;
            else
            {
                targetTransform = Object.Instantiate(_aimTargetContainer,
                    new Vector3(Random.Range(0, 3), Random.Range(0, 4), Random.Range(-3, 3)),
                    Quaternion.identity, _aimTargetContainer.parent);
            }
            
            return new WeightedTransform(targetTransform, 1);
        }

        private void ClearOldAndSetNewAimTarget(MultiAimConstraint aimConstraint, WeightedTransform weightedTransform)
        {
            Transform transformToDelete = aimConstraint.data.sourceObjects.GetTransform(0);

            foreach (RigLayer rigLayer in _rigBuilder.layers) 
                rigLayer.active = false;

            aimConstraint.data.sourceObjects = new WeightedTransformArray { weightedTransform };
            
            if (transformToDelete != _aimTargetContainer)
                Object.Destroy(transformToDelete.gameObject);

            _rigBuilder.Build();
            
            foreach (RigLayer rigLayer in _rigBuilder.layers) 
                rigLayer.active = true;
        }
    }
}