using UnityEngine;

public class BlockScript : MonoBehaviour
{
    protected PlayerCondition sPlayer;

    void Start()
    {
        sPlayer = GetComponent<PlayerCondition>();
    }

    public void PerformDeffend(bool codisi)
    {
        if (sPlayer.diGrab || sPlayer.isKnock || sPlayer.specialAttacking || sPlayer.isJumping || sPlayer.ngeGrab)
            return;
        if (codisi)
        {
            sPlayer.isBlocking = true;
            sPlayer.animator.SetBool("isBlocking", sPlayer.isBlocking);

        }
        else
        {
            sPlayer.isBlocking = false;
            sPlayer.animator.SetBool("isBlocking", sPlayer.isBlocking);
        }

    }
}
