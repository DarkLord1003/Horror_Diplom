using UnityEngine;

[System.Serializable]
public class IKLootAt
{
    [Header("LootAt Parameters")]
    [SerializeField] private bool _enabled;
    [SerializeField] [Range(0f, 1f)] private float _weight;
    [SerializeField] [Range(0f, 1f)] private float _bodyWeight;
    [SerializeField] [Range(0f, 1f)] private float _headWeight;
    [SerializeField] [Range(0f, 1f)] private float _eyesWeight;
    [SerializeField] [Range(0f, 1f)] private float _clampWeight;
    [SerializeField] private Transform _lookAtObject;

    public void LookAt(Animator animator)
    {
        if (animator == null || _lookAtObject == null || !_enabled)
            return;

        animator.SetLookAtWeight(_weight, _bodyWeight, _headWeight, _eyesWeight);
        animator.SetLookAtPosition(_lookAtObject.position);
    }
}
