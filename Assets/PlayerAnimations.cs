using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private Animator _handAnimator;

    private Player _player;

    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    private void OnEnable()
    {
        _player.Thrown += OnThrown;
    }

    private void OnDisable()
    {
        _player.Thrown -= OnThrown;
    }

    private void OnThrown()
    {
        _handAnimator.SetTrigger(HandAnimatorController.States.Throw);
    }
}