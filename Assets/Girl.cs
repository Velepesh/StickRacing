using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.Demos;

public class Girl : MonoBehaviour
{
    [SerializeField] private StickSpawner _stickSpawner;
    [SerializeField] private PendulumExample _pendulumExample;

    private void OnEnable()
    {
        _stickSpawner.StickConnected += OnStickConnected;
    }

    private void OnDisable()
    {
        _stickSpawner.StickConnected -= OnStickConnected;
    }

    private void OnStickConnected(Stick target)
    {
        _pendulumExample.StartInteractionWithStick(target.gameObject, target.ConnectToStickPoint);
    }
}