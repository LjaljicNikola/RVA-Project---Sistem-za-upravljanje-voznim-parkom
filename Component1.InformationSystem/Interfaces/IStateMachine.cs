namespace Component1.InformationSystem.Interfaces
{
    public interface IStateMachine<T>
    {
        T Advance(T currentState);
    }
}
