using System;
using System.Collections;
using Mirror;
using UnityEngine;

namespace CodeBase.Player
{
    public class PlayerAbility : NetworkBehaviour
    {
        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private Animator anim;
        [SerializeField, Range(25, 250)] private float powerDash;
        [SerializeField] private CharacterController character;
        [SerializeField] private float cooldownDash;

        private float _elapsedCooldownDash;
        private bool _isActiveAbility;
        private Vector3 _impact = Vector3.zero;
        private const float Mass = 1f;
        private static readonly int IsAttack = Animator.StringToHash("isAttack");

        private void Start()
        {
            _elapsedCooldownDash = cooldownDash;
        }

        private void Update()
        {
            Move();
            
            if (Input.GetMouseButtonDown(0) && !_isActiveAbility) 
                AddImpact(playerMovement.currentDirection, powerDash);

            if (_isActiveAbility)
            {
                _elapsedCooldownDash -= Time.deltaTime;
                if (_elapsedCooldownDash <= 0f)
                {
                    ResetAbility();
                    _elapsedCooldownDash = cooldownDash;
                }
            }

            ConsumeEnergy();
        }

        private void Move()
        {
            if (_impact.magnitude > 0.2f)
                character.Move(_impact * Time.deltaTime);
        }

        private void AddImpact(Vector3 dir, float force)
        {
            _isActiveAbility = true;
            dir.Normalize();
            if (dir.y < 0) 
                dir.y = -dir.y;
            _impact += dir.normalized * force / Mass;
            anim.SetTrigger(IsAttack);
        }

        private void ConsumeEnergy() => _impact = Vector3.Lerp(_impact, Vector3.zero, 5 * Time.deltaTime);
        private void ResetAbility() => _isActiveAbility = false;
    }
}