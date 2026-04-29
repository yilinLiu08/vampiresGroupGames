using UnityEngine;

public class BattleCharacterAnimator : MonoBehaviour
{
    public Animator animator;

    private bool isDead;

    private static readonly int AttackHash = Animator.StringToHash("Attack");
    private static readonly int DamagedHash = Animator.StringToHash("Damaged");
    private static readonly int IsDeadHash = Animator.StringToHash("IsDead");

    void Awake()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    public void SyncDeadState(bool dead)
    {
        if (animator == null)
        {
            return;
        }

        isDead = dead;
        animator.SetBool(IsDeadHash, dead);

        if (!dead)
        {
            animator.ResetTrigger(AttackHash);
            animator.ResetTrigger(DamagedHash);
        }
    }

    public void PlayAttack()
    {
        if (animator == null)
        {
            return;
        }

        if (isDead)
        {
            return;
        }

        animator.ResetTrigger(DamagedHash);
        animator.ResetTrigger(AttackHash);
        animator.SetTrigger(AttackHash);
    }

    public void PlayDamaged()
    {
        if (animator == null)
        {
            return;
        }

        if (isDead)
        {
            return;
        }

        animator.ResetTrigger(AttackHash);
        animator.ResetTrigger(DamagedHash);
        animator.SetTrigger(DamagedHash);
    }

    public void PlayDeath()
    {
        if (animator == null)
        {
            return;
        }

        if (isDead)
        {
            return;
        }

        isDead = true;

        animator.ResetTrigger(AttackHash);
        animator.ResetTrigger(DamagedHash);
        animator.SetBool(IsDeadHash, true);
    }

    public void Revive()
    {
        SyncDeadState(false);
    }
}