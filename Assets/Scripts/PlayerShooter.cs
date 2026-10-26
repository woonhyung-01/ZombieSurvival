using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    public static readonly int IdReload = Animator.StringToHash("Reload");
    public Gun gun;
    public Transform gunPivot;
    public Transform leftHandMouht;
    public Transform rightHandMouht;

    private Animator playerAnimator;
    private PlayerInput playerInput;

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        if (playerInput.Fire)
        {
            gun.Fire();
        }
        else if (playerInput.Reload && gun.Reload())
        {
            playerAnimator.SetTrigger(IdReload);
        }
        
    }

    private void OnAnimatorIK(int layerIndex)
    {
        gunPivot.position = playerAnimator.GetIKHintPosition(AvatarIKHint.RightElbow);

        playerAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
        playerAnimator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
        playerAnimator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandMouht.position);
        playerAnimator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandMouht.rotation);

        playerAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
        playerAnimator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);
        playerAnimator.SetIKPosition(AvatarIKGoal.RightHand, rightHandMouht.position);
        playerAnimator.SetIKRotation(AvatarIKGoal.RightHand, rightHandMouht.rotation);
    }
}
