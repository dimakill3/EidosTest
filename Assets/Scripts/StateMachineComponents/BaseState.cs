using _Assets.Scripts.Core.StateMachine.StateInterfaces;

namespace StateMachineComponents
{
    public abstract class BaseState : IState
    {
        protected readonly StateMachine _stateMachine;
        
        public BaseState(StateMachine stateMachine) =>
            _stateMachine = stateMachine;
        
        public virtual void Enter()
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