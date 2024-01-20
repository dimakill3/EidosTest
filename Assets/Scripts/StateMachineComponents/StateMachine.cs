using System;
using System.Collections.Generic;
using _Assets.Scripts.Core.StateMachine.StateInterfaces;

namespace StateMachineComponents
{
    public class StateMachine
    {
        private readonly Dictionary<Type, IExitableState> _states = new();
        private IExitableState _currentState;
        private bool _enabled = true;

        public void Disable()
        {
            _enabled = false;
            _currentState?.Exit();
        }
        
        public void AddState<TState>(TState state) where TState : IExitableState => 
            _states.Add(typeof(TState), state);

        public void EnterState<TState>() where TState : class, IState
        {
            IState state = ChangeState<TState>();
            state.Enter();
            _enabled = true;
        }

        public void EnterState<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
        {
            TState state = ChangeState<TState>();
            state.Enter(payload);
        }

        public void Update()
        {
            if(!_enabled)
                return;
            _currentState.Update();
        }

        public void FixedUpdate()
        {
            if(!_enabled)
                return;
            _currentState.FixedUpdate();
        }

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _currentState?.Exit();
            TState state = GetState<TState>();
            _currentState = GetState<TState>();;
            return state;
        }

        private TState GetState<TState>() where TState : class, IExitableState => 
            _states[typeof(TState)] as TState;
    }
}