using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerStateMachine : StateMachine
{
    private int _isWalkingHash = Animator.StringToHash("IsWalking");
    private int _isSprintingHash = Animator.StringToHash("IsSprinting");
    private int _isJumpingHash = Animator.StringToHash("IsJumping");
    private int _isCrouchingHash = Animator.StringToHash("IsCrouching");
    private int _isLandingHash = Animator.StringToHash("IsLanding");
    private int _xVelocityHash = Animator.StringToHash("VelocityX");
    private int _yVelocityHash = Animator.StringToHash("VelocityY");

    private float _velocityX;
    private float _velocityY;
    private bool _isWalking;
    private bool _isSprinting;
    private bool _isCrouching;

    private Action _onJump;
    private Action _onLand;

    public float VelocityX
    {
        set => _velocityX = value;
    }

    public float VelocityY
    {
        set => _velocityY = value;
    }

    public bool IsWalking
    {
        set => _isWalking = value;
    }

    public bool IsSprinting
    {
        set => _isSprinting = value;
    }

    public bool IsCrouching
    {
        set => _isCrouching = value;
    }

    public Action OnJump => _onJump;
    public Action OnLand => _onLand;

    private void Start()
    {
        if (States == null)
            return;

        CurrentStateType = StateType.Idle;

        if (States.ContainsKey(CurrentStateType))
        {
            CurrentState = States[CurrentStateType];
        }
        else
        {
            CurrentStateType = StateType.None;
            CurrentState = null;
        }
    }
    public override void Update()
    {
        base.Update();

        if (Animator)
        {
            Animator.SetFloat(_xVelocityHash, _velocityX);
            Animator.SetFloat(_yVelocityHash, _velocityY);

            Animator.SetBool(_isWalkingHash, _isWalking);
            Animator.SetBool(_isSprintingHash, _isSprinting);
            Animator.SetBool(_isCrouchingHash, _isCrouching);
        }
    }

    private void OnJumping()
    {
        ActiveAnimTrigger(_isJumpingHash);
    }

    private void OnLanding()
    {
        ActiveAnimTrigger(_isLandingHash);
    }

    private void ActiveAnimTrigger(int paramHash)
    {
        if (Animator == null)
            return;

        Animator.SetTrigger(paramHash);
    }

    #region - OnEnable/OnDisable

    private void OnEnable()
    {
        _onJump += OnJumping;
        _onLand += OnLanding;
    }

    private void OnDisable()
    {
        _onJump -= OnJumping;
        _onLand -= OnLanding;
    }

    #endregion
}

public enum StateType
{
    None,
    Idle,
    Walking,
    Sprinting,
    Jumping,
    Landing,
    Crouching
}
