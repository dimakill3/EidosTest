using _Assets.Scripts.Core.StateMachine.StateInterfaces;

namespace _Assets.Scripts.Core.StateMachine
{
    public class BasePayloadedState<TPayload> : IPayloadedState<TPayload>
    {
        protected readonly global::StateMachineComponents.StateMachine _stateMachine;
        
        public BasePayloadedState(global::StateMachineComponents.StateMachine stateMachine) =>
            _stateMachine = stateMachine;

        public virtual void Enter(TPayload payload)
        {
        }
        
        public virtual void Exit()
        {
        }

        public void Update()
        {
        }

        public void FixedUpdate()
        {
        }
    }
}