using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private StickSpawner _stickSpawner;

    private Stick _currentStick;

    private void Start()
    {
        _currentStick = _stickSpawner.CurrentStick;
    }

    public void ThrowStick(Vector3 targetPoint)
    {
        _currentStick.Throwing(targetPoint);

        TakeAnotherStick();
    }

    private void TakeAnotherStick()
    {
        //_currentStick = null;
        _currentStick = _stickSpawner.SpawnNewStick();
    }
}
