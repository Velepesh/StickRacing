using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [SerializeField] private StickSpawner _stickSpawner;

    public event UnityAction Thrown;

    public void ThrowStick(Vector3 targetPoint)
    {
        Stick stick = _stickSpawner.CurrentStick;

        if (stick != null)
            stick.Throwing(targetPoint);

        _stickSpawner.SpawnNewStick();

        Thrown?.Invoke();
    }
}