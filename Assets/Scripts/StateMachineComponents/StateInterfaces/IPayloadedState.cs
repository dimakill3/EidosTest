namespace _Assets.Scripts.Core.StateMachine.StateInterfaces
{
    public interface IPayloadedState<TPayload> : IExitableState
    {
        public void Enter(TPayload payload);
    }
}