using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    [Header("State")]
    public bool diGrab = false;
    public bool ngeGrab = false;
    public bool isKnock = false;
    public bool isJumping = false;
    public bool isBlocking = false;
    public bool isAttack = false;
    public bool isHurt = false;
    public bool specialAttacking = false;

    [Header("Components")]
    [SerializeField] public Animator animator;
    [SerializeField] public Rigidbody2D rigidbody2D;



    public void FalseAllAnimation()
    {
        ngeGrab = false;
        animator.SetBool("isGrabing", ngeGrab);

        isKnock = false;
        animator.SetBool("isKnock", isKnock);

        isJumping = false;
        animator.SetBool("isJumping", isJumping);

        isBlocking = false;
        animator.SetBool("isBlocking", isBlocking);

        isAttack = false;
        animator.SetBool("isAttacking", isAttack);

        specialAttacking = false;
        animator.SetBool("isSpecialAtk", specialAttacking);

        isHurt = false;
        animator.SetBool("isHurt", isHurt);
    }

}
