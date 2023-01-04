using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IKHand
{

    [Header("Active")]
    [SerializeField] private bool _enabled;

    [Header("Right Hand Parameters")]
    [SerializeField] private Transform _rightHandTarget;
    [SerializeField] private Transform _rightHintTarget;
    [SerializeField] [Range(0f, 1f)] private float _rightHandWeight;
    [SerializeField] [Range(0f, 1f)] private float _rightHintWeight;

    [Header("Left Hand Parameters")]
    [SerializeField] private Transform _leftHandTarget;
    [SerializeField] private Transform _leftHintTarget;
    [SerializeField] [Range(0f, 1f)] private float _leftHandWeight;
    [SerializeField] [Range(0f, 1f)] private float _leftHintWeight;

    public void UpdateIKHand(Animator animator)
    {
        if (animator == null || !_enabled)
            return;

        if (_rightHandTarget == null || _leftHandTarget == null || _rightHintTarget == null || _leftHintTarget == null)
            return;

        SetIKHandParameters(animator, AvatarIKGoal.RightHand, _rightHandWeight, _rightHandTarget);
        SetIKHintParameters(animator, AvatarIKHint.RightElbow, _rightHintWeight, _rightHintTarget);

        SetIKHandParameters(animator, AvatarIKGoal.LeftHand, _leftHandWeight, _leftHandTarget);
        SetIKHintParameters(animator, AvatarIKHint.LeftElbow, _leftHintWeight, _leftHintTarget);
    }

    private void SetIKHandParameters(Animator animator,AvatarIKGoal avatarIKGoal, float weight, Transform target)
    {
        animator.SetIKPositionWeight(avatarIKGoal, weight);
        animator.SetIKPosition(avatarIKGoal, target.position);

        animator.SetIKRotationWeight(avatarIKGoal, weight);
        animator.SetIKRotation(avatarIKGoal, target.rotation);
    }

    private void SetIKHintParameters(Animator animator, AvatarIKHint avatarIKHint, float weight, Transform target)
    {
        animator.SetIKHintPositionWeight(avatarIKHint, weight);
        animator.SetIKHintPosition(avatarIKHint, target.position);
    }
}
