using Mirror;
using UnityEngine;

namespace CodeBase.Player
{
    public class PlayerAnimator : NetworkBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private NetworkAnimator networkAnimator;
        
        private static readonly int IsWalk = Animator.StringToHash("isWalk");
        private static readonly int IsAttack = Animator.StringToHash("isAttack");

        public void PlayWalk() => animator.SetBool(IsWalk, true);
        public void StopWalk() => animator.SetBool(IsWalk, false);
        public void PlayAttack() => networkAnimator.SetTrigger(IsAttack);
    }
}