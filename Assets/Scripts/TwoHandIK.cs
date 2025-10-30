using UnityEngine;

public class TwoHandIK : MonoBehaviour
{
    public Animator animator;
    public Transform leftHandTarget;

    void OnAnimatorIK(int layerIndex)
    {
        if (leftHandTarget == null) return;

        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
        animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandTarget.position);
        animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandTarget.rotation);
    }
}
