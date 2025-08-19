using UnityEngine;

public interface IState<T> where T : MonoBehaviour
{
    void Handle(T context);
    void Enter();
    void Action();
    void Exit();
}
