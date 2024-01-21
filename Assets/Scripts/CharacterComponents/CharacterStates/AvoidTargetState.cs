using _Assets.Scripts.Core.StateMachine;
using Services;
using StateMachineComponents;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace CharacterComponents.CharacterStates
{
    public class AvoidTargetState : BasePayloadedState<Transform>
    {
        private readonly Transform _bodyAvoidConstraint;
        private readonly Transform _headAvoidConstraint;
        private readonly Rig _bodyAvoidRig;
        private readonly Rig _headAvoidRig;
        private readonly float _avoidBodyConstraintZAxisRange;
        private readonly float _avoidBodyConstraintXAxisRange;
        private readonly float _avoidHeadConstraintZAxisRange;
        private readonly float _avoidHeadConstraintXAxisRange;
        private readonly Vector3 _characterHeadDefaultPosition;
        private readonly Vector3 _characterBodyDefaultPosition;
        private readonly Vector3 _halfHeadSize;
        private readonly Vector3 _halfBodySize;
        private readonly MonoService _monoService;
        
        private Transform _avoidTarget;

        public AvoidTargetState(StateMachine stateMachine, Transform bodyAvoidConstraint,
            Transform headAvoidConstraint, Rig bodyAvoidRig, Rig headAvoidRig, float avoidBodyConstraintZAxisRange,
            float avoidBodyConstraintXAxisRange, float avoidHeadConstraintZAxisRange,
            float avoidHeadConstraintXAxisRange, Transform characterBodyTransform, Transform characterHeadTransform,
            Vector3 halfHeadSize, Vector3 halfBodySize, MonoService monoService) : base(stateMachine)
        {
            _bodyAvoidConstraint = bodyAvoidConstraint;
            _headAvoidConstraint = headAvoidConstraint;
            _bodyAvoidRig = bodyAvoidRig;
            _headAvoidRig = headAvoidRig;
            _avoidBodyConstraintZAxisRange = avoidBodyConstraintZAxisRange;
            _avoidBodyConstraintXAxisRange = avoidBodyConstraintXAxisRange;
            _avoidHeadConstraintZAxisRange = avoidHeadConstraintZAxisRange;
            _avoidHeadConstraintXAxisRange = avoidHeadConstraintXAxisRange;
            _characterHeadDefaultPosition = characterHeadTransform.position;
            _characterBodyDefaultPosition = characterBodyTransform.position;
            _halfHeadSize = halfHeadSize * 1.25f;
            _halfBodySize = halfBodySize * 1.25f;
            _monoService = monoService;
        }

        public override void Enter(Transform target)
        {
            _avoidTarget = target;

            _bodyAvoidRig.weight = 1;
            _headAvoidRig.weight = 1;
            
            _monoService.UpdateTick += OnTickUpdate;
        }

        public override void Exit()
        {
            _monoService.UpdateTick -= OnTickUpdate;
            
            _bodyAvoidRig.weight = 0;
            _headAvoidRig.weight = 0;
        }

        private void OnTickUpdate() => 
            AvoidTarget();

        private void AvoidTarget()
        {
            Vector3 distanceFromTargetToHead = _avoidTarget.position - _characterHeadDefaultPosition;
            Vector3 distanceFromTargetToBody = _avoidTarget.position - _characterBodyDefaultPosition;

            if (Mathf.Abs(distanceFromTargetToHead.x) <= _halfHeadSize.x &&
                Mathf.Abs(distanceFromTargetToHead.y) <= _halfHeadSize.y &&
                Mathf.Abs(distanceFromTargetToHead.z) <= _halfHeadSize.z)
            {
                float normalized = (distanceFromTargetToHead.x + _halfHeadSize.x) / (_halfHeadSize.x * 2);
                float halfNormalized = Mathf.Abs(normalized - 0.5f);
                float invertedToCenter = distanceFromTargetToHead.x > 0
                    ? 1 - halfNormalized
                    : halfNormalized;

                float xCoord = -Mathf.Lerp(-_avoidHeadConstraintXAxisRange, _avoidHeadConstraintXAxisRange,
                    invertedToCenter);
                
                if (distanceFromTargetToHead.x == 0)
                    xCoord = 0;
                
                normalized = (distanceFromTargetToHead.z + _halfHeadSize.z) / (_halfHeadSize.z * 2);
                halfNormalized = Mathf.Abs(normalized - 0.5f);
                invertedToCenter = distanceFromTargetToHead.z > 0
                    ? 1 - halfNormalized
                    : halfNormalized;
                
                float zCoord = -Mathf.Lerp(-_avoidHeadConstraintZAxisRange, _avoidHeadConstraintZAxisRange,
                    invertedToCenter);

                if (distanceFromTargetToHead.z == 0)
                    zCoord = 0;
                
                _headAvoidConstraint.position = new Vector3(xCoord, _headAvoidConstraint.position.y, zCoord);
            }
            else
                _headAvoidConstraint.position = new Vector3(0, _headAvoidConstraint.position.y, 0);

            if (Mathf.Abs(distanceFromTargetToBody.x) <= _halfBodySize.x &&
                Mathf.Abs(distanceFromTargetToBody.y) <= _halfBodySize.y &&
                Mathf.Abs(distanceFromTargetToBody.z) <= _halfBodySize.z)
            {
                float normalized = (distanceFromTargetToBody.x + _halfBodySize.x) / (_halfBodySize.x * 2);
                float halfNormalized = Mathf.Abs(normalized - 0.5f);
                float invertedToCenter = distanceFromTargetToBody.x > 0
                    ? 1 - halfNormalized
                    : halfNormalized;

                float xCoord = -Mathf.Lerp(-_avoidBodyConstraintXAxisRange, _avoidBodyConstraintXAxisRange,
                    invertedToCenter);
                
                if (distanceFromTargetToBody.x == 0)
                    xCoord = 0;
                
                normalized = (distanceFromTargetToBody.z + _halfBodySize.z) / (_halfBodySize.z * 2);
                halfNormalized = Mathf.Abs(normalized - 0.5f);
                invertedToCenter = distanceFromTargetToBody.z > 0
                    ? 1 - halfNormalized
                    : halfNormalized;
                
                float zCoord = -Mathf.Lerp(-_avoidBodyConstraintZAxisRange, _avoidBodyConstraintZAxisRange,
                    invertedToCenter);

                if (distanceFromTargetToBody.z == 0)
                    zCoord = 0;
                
                _bodyAvoidConstraint.position = new Vector3(xCoord, _bodyAvoidConstraint.position.y, zCoord);
            }
            else
                _bodyAvoidConstraint.position = new Vector3(0, _bodyAvoidConstraint.position.y, 0);
        }
    }
}