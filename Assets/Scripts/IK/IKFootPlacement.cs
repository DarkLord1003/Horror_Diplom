using UnityEngine;

[System.Serializable]
public class IKFootPlacement
{
    [Header("Foot Placement Parameters")]
    [SerializeField] private bool _enabled;
    [SerializeField] [Range(0f, 10f)] private float _distanceToGround;
    [SerializeField] private LayerMask _mask;

    [Header("Animation Variable Name")]
    [SerializeField] private string _rightFoot;
    [SerializeField] private string _leftFoot;

    private Animator _animator;

    public void Init(Animator animator)
    {
        _animator = animator;
    }

    public void UpdateFoot()
    {
        if (!_enabled || _animator == null)
            return;

        RaycastToGround(AvatarIKGoal.RightFoot, _rightFoot, _distanceToGround, _mask);
        RaycastToGround(AvatarIKGoal.LeftFoot, _leftFoot, _distanceToGround, _mask);
    }

    public void RaycastToGround(AvatarIKGoal avatarIKGoal,string footName,float distaceToGround,LayerMask mask)
    {
        float weight = _animator.GetFloat(footName);

        _animator.SetIKPositionWeight(avatarIKGoal, weight);

        RaycastHit hit;
        Vector3 footPosition = _animator.GetIKPosition(avatarIKGoal) + Vector3.up;
        Ray ray = new Ray(footPosition, Vector3.down);
        if(Physics.Raycast(ray,out hit,distaceToGround + 1f, mask))
        {
            Vector3 newFootPosition = hit.point;
            newFootPosition.y += distaceToGround;

            _animator.SetIKPosition(avatarIKGoal, newFootPosition);
        }

    }
}
