namespace _Assets.Scripts.Core.StateMachine.StateInterfaces
{
    public interface IExitableState
    {
        public void Exit();
        public void Update();
        public void FixedUpdate();
    }
}