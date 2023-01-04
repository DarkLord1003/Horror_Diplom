using UnityEngine;

[RequireComponent(typeof(Animator))]
public class IKController : MonoBehaviour
{
    [SerializeField] private IKLootAt _ikLookAt;
    [SerializeField] private IKFootPlacement _IKFootPlacement;
    [SerializeField] private IKHand _IKHand;


    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _IKFootPlacement.Init(_animator);
    }

    private void OnAnimatorIK(int layerIndex)
    {
        _ikLookAt.LookAt(_animator);
        _IKFootPlacement.UpdateFoot();
        _IKHand.UpdateIKHand(_animator);
    }
}
