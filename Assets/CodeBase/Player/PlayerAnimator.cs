using Mirror;
using UnityEngine;

namespace CodeBase.Player
{
    public class PlayerAnimator : NetworkBehaviour
    {
        public Animator anim;
        public NetworkAnimator networkAnimator;
        
        private static readonly int IsWalk = Animator.StringToHash("isWalk");
        private static readonly int IsAttack = Animator.StringToHash("isAttack");

        public void PlayWalk() => anim.SetBool(IsWalk, true);
        public void StopWalk() => anim.SetBool(IsWalk, false);
        public void PlayAttack() => networkAnimator.SetTrigger(IsAttack);
    }
}